<?php
    /*
      请求接口格式：（更详细可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType）
        - uid：必填，用户id；
        - pwd：必填，用户密码；
        - file_name: 必填，简历文件名（请确保后缀正确）；
        - file_cont: 必填，经based64编码的简历文件内容；
        - need_avatar: 可选，是否需要解析头像，0为不需要，1为需要，默认为0；
        - 其他可选字段可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType
    */
    $url ="http://www.resumesdk.com/api/parse";     // 接口地址，也可以用https

    $file_name = 'D:/resumeSDK/resume.docx';       // 替换为你的本地文件名
    $file_cont = base64_encode(file_get_contents($file_name));
    $data = array(
        'file_cont' => $file_cont,
        'file_name'=> $file_name,
        'need_avatar' => 0
    );
    $data_string = json_encode($data);

    $headers = array(
        'Content-Type: application/json',
        'uid: 123456',      //替换为您的uid
        'pwd: 123abc'       //替换为您的pwd   
    );

    $ch = curl_init();
    curl_setopt($ch , CURLOPT_URL , $url);
    curl_setopt($ch , CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch , CURLOPT_POST, 1);
    curl_setopt($ch , CURLOPT_POSTFIELDS, $data_string);
    curl_setopt($ch , CURLOPT_HTTPHEADER, $headers);
    $output = curl_exec($ch);
    $httpcode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    echo 'HTTP status code: ' . $httpcode . "\n";
    print_r($output);
?>
