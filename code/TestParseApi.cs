using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class StatusObj {
	public string message {get;set;}
	public int code {get;set;}
}

public class AccountObj {
	public int uid {get;set;}
	public int usage_limit {get;set;}
	public int usage_remaining {get;set;}
}

public class EducationObj {
	public string start_date {get;set;}
	public string end_date {get;set;}
	public string edu_college {get;set;}
	public string edu_college_dept {get;set;}
	public string edu_major {get;set;}
	public string edu_degree {get;set;}
	public string edu_degree_norm {get;set;}
}

public class JobExpObj {
	public string start_date {get;set;}
	public string end_date {get;set;}
	public string job_cpy {get;set;}
	public string job_cpy_nature {get;set;}
	public string job_cpy_size {get;set;}
	public string job_industry {get;set;}
	public string job_position {get;set;}
	public string job_cpy_dept {get;set;}
	public string job_nature {get;set;}
	public string job_salary {get;set;}
	public string job_staff {get;set;}
	public string job_report_to {get;set;}
	public string job_location {get;set;}
	public string job_why_leave {get;set;}
	public string job_duaraton {get;set;}
	public string job_content {get;set;}
}

public class ProjExpObj {
	public string start_date {get;set;}
	public string end_date {get;set;}
	public string proj_name {get;set;}
	public string proj_position {get;set;}
	public string proj_content {get;set;}
	public string proj_resp {get;set;}
}

public class ResultObj {
	public string name {get;set;}
	public string email {get;set;}
	public string phone {get;set;}
	public string gender {get;set;}
	public string age {get;set;}
	public string city {get;set;}
	public string height {get;set;}
	public string weight {get;set;}
	public string marital_status {get;set;}
	public string birthday {get;set;}
	public string living_address {get;set;}
	public string hukou_address {get;set;}
	public string hometown_address {get;set;}
	public string qq {get;set;}
	public string race {get;set;}
	public string nationality {get;set;}
	public string postal_code {get;set;}
	public string polit_status {get;set;}
	public string english {get;set;}
	public string work_year {get;set;}
	public string work_start_time {get;set;}
	public string work_position {get;set;}
	public string work_company {get;set;}
	public string grad_time {get;set;}
	public string college {get;set;}
	public string college_dept {get;set;}
	public string major {get;set;}
	public string degree {get;set;}
	public string expect_job {get;set;}
	public string expect_cpy {get;set;}
	public string expect_salary {get;set;}
	public string expect_industry {get;set;}
	public string expect_time {get;set;}
	public string expect_jnature {get;set;}
	public string expect_jlocation {get;set;}

	public List<EducationObj> education_objs {get;set;}
	public List<JobExpObj> job_exp_objs {get;set;}
	public List<ProjExpObj> proj_exp_objs {get;set;}

	public string social_exp {get;set;}
	public string job_skill {get;set;}
	public string my_desc {get;set;}
	//public string raw_text {get;set;}

	// 更多字段及定义请参考《接口文档》
	// ...
}

public class ParseResObj {
	public StatusObj status {get;set;}
	public AccountObj account {get;set;}
	public ResultObj result {get;set;}
}

class TestParserApi {
	static public string Base64Encode(string s) {
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
		string res = Convert.ToBase64String(bytes);

		return res;
	}

	static public void PrintBasicInfo(ResultObj res) {
		/**
			打印基本信息字段
		**/
		Console.WriteLine("{");
		foreach(var prop in res.GetType().GetProperties()) {
			if(!prop.Name.EndsWith("_objs")) {
				if(prop.GetValue(res, null) != null) {
					Console.WriteLine("    {0}: {1}", prop.Name, prop.GetValue(res, null));
				}	
			}
		}
		Console.WriteLine("}");
	}

	static public void TestParser(string url, string fname, int uid, string pwd) {
		string html = string.Empty;
		
		// 构造请求头
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = "POST";
		request.ContentType = "application/json";

		// 读取文件内容，经过base64编码后发送
		Byte[] bytes = File.ReadAllBytes(fname);
		String base_cont = Convert.ToBase64String(bytes);

		using (var streamWriter = new StreamWriter(request.GetRequestStream())){
		    string json = new JavaScriptSerializer().Serialize(new
		                {
		                    file_cont = base_cont,	// 经base64编码的文件内容
		                    file_name = fname,	// 文件名称
		                    uid = uid, 		// 用户id
		                    pwd = pwd		// 密码
		                });

		    streamWriter.Write(json);
		}

		// 获取请求结果：json格式
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		using (var streamReader = new StreamReader(response.GetResponseStream())){
			html = streamReader.ReadToEnd();
			//Console.WriteLine(html);
		}

		// 解析并打印json结果（此处用JavaScriptSerializer解析，也可用Newtonsoft.Json等）
		var jss = new JavaScriptSerializer();
		ParseResObj res = jss.Deserialize<ParseResObj>(html);
		if(res.status.code != 200) {
			Console.WriteLine("parse resume <{0}> failed: code=<{1}>, message=<{2}>", fname, res.status.code, res.status.message);
		}
		else {
			PrintBasicInfo(res.result);
			if(!string.IsNullOrEmpty(res.result.name)) {
				Console.WriteLine("name is: " + res.result.name);
			}
			Console.WriteLine("usage_remaining is: " + res.account.usage_remaining);
			Console.WriteLine("parse resume <{0}> succeeded: code=<{1}>, message=<{2}>", fname, res.status.code, res.status.message);
		}
	}

    static public void Main(){
    	string url = "http://www.resumesdk.com/api/parse";
    	string fname = "D:/test_input/resume.docx";	// 替换为你的简历文件名，确保后缀名正确
    	int uid = 123;			// 替换为你的uid
    	string pwd = "123abc";		// 替换为你的pwd
    	TestParser(url, fname, uid, pwd);
    }
}