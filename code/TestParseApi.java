package com.xxx.parser.demo;

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

    public static void testResumeParser(String url, String fname, int uid, String pwd) throws Exception {
    	// 设置头字段
        HttpPost httpPost = new HttpPost(url);
        httpPost.addHeader("content-type", "application/json");
        
        // 读取简历内容
    	byte[] bytes = FileUtils.readFileToByteArray(new File(fname));
    	String data = new String(Base64.encodeBase64(bytes), Consts.UTF_8);
    	
        // 设置内容信息
        JSONObject json = new JSONObject();
        json.put("uid", uid);			// 用户id
        json.put("pwd", pwd);			// 用户密码
        json.put("file_name", fname);	// 文件名
        json.put("file_cont", data);	// 经base64编码过的文件内容
        StringEntity params = new StringEntity(json.toString(), Consts.UTF_8);
        httpPost.setEntity(params);
        
        // 发送请求
        HttpClient httpclient = new DefaultHttpClient(); 
        HttpResponse response = httpclient.execute(httpPost);
        
        // 处理返回结果
        String resCont = EntityUtils.toString(response.getEntity(), Consts.UTF_8);
        System.out.println(resCont);
        
        JSONObject res = new JSONObject(resCont); 
        JSONObject status = res.getJSONObject("status");
        if(status.getInt("code") != 200) {
        	System.out.println("request failed: code=<" + status.getInt("code") + ">, message=<" + status.getString("message") + ">");
        }
        else {
        	JSONObject acc = res.getJSONObject("account");
        	System.out.println("usage_remaining:" + acc.getInt("usage_remaining"));
        	
        	JSONObject result = res.getJSONObject("result");
        	System.out.println("result:\n" + result.toString(4));
        	System.out.println("request succeeded");
        }
    }
    
    public static void main(String[] args) throws Exception {
        String url = "http://www.resumesdk.com/api/parse";	
        String fname = "D:/test_files/resume.doc";	// 替换为你的简历文件名，确保后缀名正确
        int uid = 123456;		// 替换为你的用户名（int格式）
        String pwd = "123abc";	//替换为你的密码（String格式）

        testResumeParser(url, fname, uid, pwd);
    }
}
