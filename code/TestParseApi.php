<?php

$url ="http://www.resumesdk.com/api/parse";

$fname = 'D:/test_input/resume.docx';		// 替换为你的简历文件名，确保后缀名正确
$uid = 123;			// 替换为你的用户名(uid)，整数类型
$pwd = '123abc';		// 替换为你的密码(pwd)，字符串类型
$fileData = file_get_contents($fname);

$base_cont = base64_encode($fileData);
$data = array(
    'file_cont' => $base_cont,		// 经base64编码过的文件内容
    'file_name'=> $fname,					// 文件名
    'uid' => $uid,					// 用户名
    'pwd' => $pwd						// 密码
);


$data = json_encode($data, JSON_UNESCAPED_UNICODE);		// 需php5.4及以上才支持JSON_UNESCAPED_UNICODE
$result = http($url, $data);
$res_js = json_decode($result, TRUE);
$name = $res_js['result']['name'];
var_dump($name);
//var_dump($res_js);
//var_dump($result);

exit;

function http($url, $data)
{
    $process = curl_init($url);
	curl_setopt($process, CURLOPT_HTTPHEADER, array('Content-Type: application/json'));
	curl_setopt($process, CURLOPT_HEADER, FALSE);
	curl_setopt($process, CURLOPT_TIMEOUT, 600);
	curl_setopt($process, CURLOPT_POST, TRUE);
	curl_setopt($process, CURLOPT_POSTFIELDS, $data);
	curl_setopt($process, CURLOPT_RETURNTRANSFER, TRUE);
	$content = curl_exec($process);
	curl_close($process);

    return $content;
}

?>