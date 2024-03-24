var request = require('request');
var fs = require('fs');

var url = 'http://www.resumesdk.com/api/parse'; // 接口地址，也可以用https
var fname = 'D:/resumeSDK/resume.docx';        // 替换为你的文件名
var uid = 123456;       // 替换为你的用户名（int格式）
var pwd = '123abc';     // 替换为你的密码（str格式）

var options = {
    url: url,
    headers: {
        'uid': uid,
        'pwd': pwd,
    },
    json: {
        'file_cont': Buffer(fs.readFileSync(fname)).toString('base64'),
        'file_name': fname,
        'need_avatar': 0
    }
};

var result = request.post(options, function(err, resp, body) {
    console.log('http status code: ', resp.statusCode);
    if (err) {
        console.log(err);
        process.exit(1);
    } else {
        console.log(body);
        process.exit(0);
    }
});
