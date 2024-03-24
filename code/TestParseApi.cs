using System;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

class TestParserApi {
  /*
    请求接口格式：
      - uid：必填，用户id；
      - pwd：必填，用户密码；
      - file_name: 必填，简历文件名（请确保后缀正确）；
      - file_cont: 必填，经based64编码的简历文件内容；
      - need_avatar: 可选，是否需要解析头像，0为不需要，1为需要，默认为0；
      - 其他可选字段可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType
  */
  static public void TestParser(string url, string fname, int uid, string pwd) {
    string json_string = string.Empty;
    
    // 构造请求头
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.Method = "POST";
    request.ContentType = "application/json";
    request.Headers.Add("uid", uid.ToString());
    request.Headers.Add("pwd", pwd);

    // 读取文件内容，经过base64编码后发送
    Byte[] bytes = File.ReadAllBytes(fname);
    String base_cont = Convert.ToBase64String(bytes);
    
    using (var streamWriter = new StreamWriter(request.GetRequestStream())){
        string json = new JavaScriptSerializer().Serialize(new
                    {
                        file_cont = base_cont,  // 经base64编码的文件内容
                        file_name = fname,  // 文件名称
                        need_avatar = 0,
                    });

        streamWriter.Write(json);
    }

    // 获取请求结果：json格式
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    Console.WriteLine("http status code: {0}", response.StatusCode);

    using (var streamReader = new StreamReader(response.GetResponseStream())){
      json_string = streamReader.ReadToEnd();
    }
    // Console.WriteLine(json_string);

    // 解析并打印json结果（此处用JavaScriptSerializer解析，也可用Newtonsoft.Json等）
    var serializer = new JavaScriptSerializer();
    dynamic js = serializer.DeserializeObject(json_string);
    Console.WriteLine("code:" + js["status"]["code"] + ", message: " + js["status"]["message"]);
    Console.WriteLine("name:" + js["result"]["name"]);
  }

  static public void Main(){
    string url = "http://www.resumesdk.com/api/parse";  // 接口地址，也可以用https
    string fname = "./resume.docx"; // 替换为你的简历文件
    int uid = 123456;      // 替换为你的uid
    string pwd = "123abc";   // 替换为你的pwd
    TestParser(url, fname, uid, pwd);
  }
}
