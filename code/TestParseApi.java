import java.io.File;
import org.apache.commons.io.FileUtils;
import org.apache.commons.codec.binary.Base64;
import org.apache.http.Consts;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONObject;

public class TestParseApi {
  /**
   * 注：需要预安装apache-http, json, commons-io
   * 请求接口格式：（更详细可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType）
   * - uid: 必填，用户id；
   * - pwd: 必填，用户密码；
   * - file_name: 必填，简历文件名（请确保后缀正确）；
   * - file_cont: 必填，经based64编码的简历文件内容；
   * - need_avatar: 可选，是否需要解析头像，0为不需要，1为需要，默认为0；
   * - 其他可选字段可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType
   */
    public static void testResumeParser(String url, String fname, int uid, String pwd) throws Exception {
        HttpPost httpPost = new HttpPost(url);

      // 设置头字段
        httpPost.setHeader("uid", String.valueOf(uid));
        httpPost.setHeader("pwd", pwd);
        httpPost.addHeader("content-type", "application/json");
        
        // 读取简历内容
      byte[] bytes = FileUtils.readFileToByteArray(new File(fname));
      String data = new String(Base64.encodeBase64(bytes), Consts.UTF_8);
      
        // 设置请求接口信息
        JSONObject json = new JSONObject();
        json.put("file_name", fname); // 文件名
        json.put("file_cont", data);  // 经base64编码过的文件内容
        json.put("need_avatar", 0); 
        StringEntity params = new StringEntity(json.toString(), Consts.UTF_8);
        httpPost.setEntity(params);
        
        // 发送请求
        HttpClient httpclient = new DefaultHttpClient(); 
        HttpResponse response = httpclient.execute(httpPost);
        
        // 处理返回结果
        int httpCode = response.getStatusLine().getStatusCode();
        System.out.println("http status code:" + httpCode);

        String resCont = EntityUtils.toString(response.getEntity(), Consts.UTF_8);
        JSONObject res = new JSONObject(resCont); 
        JSONObject status = res.getJSONObject("status");
        if(status.getInt("code") != 200) {
            System.out.println("request failed: code=<" + status.getInt("code") + ">, message=<" + status.getString("message") + ">");
        }
        else {
            JSONObject result = res.getJSONObject("result");
            System.out.println("result:\n" + result.toString(4));
            System.out.println("request succeeded");
        }
    }
    
    public static void main(String[] args) throws Exception {
        String url = "http://www.resumesdk.com/api/parse";    // 接口地址，也可以用https
        String fname = "D:/resumeSDK/resume.docx";  //替换为你的文件名
        int uid = 123456;   //替换为你的用户名（int格式）
        String pwd = "123abc";  //替换为你的密码（String格式）

        testResumeParser(url, fname, uid, pwd);
    }
}
