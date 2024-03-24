#coding: utf-8

import sys
import base64
import requests
import json

def test_parser(url, fname, uid, pwd):
    """
    请求简历解析接口，请求字段：
    - uid：必填，用户id；
    - pwd：必填，用户密码；
    - file_name: 必填，简历文件名（请确保后缀正确）；
    - file_cont: 必填，经based64编码的简历文件内容；
    - need_avatar: 可选，是否需要解析头像，0为不需要，1为需要，默认为0；
    - 其他可选字段可参考官网：https://www.resumesdk.com/docs/rs-parser.html#reqType
    """

    # 读取文件内容，构造请求
    cont = open(fname, 'rb').read()
    base_cont = base64.b64encode(cont)
    base_cont = base_cont.decode('utf-8') if sys.version.startswith('3') else base_cont     #兼容python2与python3
    
    headers = {'uid': str(uid), 'pwd': pwd}

    data = {'file_name': fname,
            'file_cont': base_cont,
            'need_avatar': 0,
            }
    
    # 发送请求
    res = requests.post(url, data=json.dumps(data), headers=headers)
    
    # 解析结果
    http_code = res.status_code
    if http_code != 200:
        print("http status code:", res.status_code)
    else:
        res_js = json.loads(res.text)
        print('result:\n%s\n'%(json.dumps(res_js, indent=2, ensure_ascii=False)))
    
if __name__ == '__main__' :
    url = 'http://www.resumesdk.com/api/parse'    # 接口地址，也可以用https
    fname = u'D:/resumeSDK/resume.docx'  # 替换为你的文件名
    uid = 123456   # 替换为你的用户名（int格式）
    pwd = '123abc'  # 替换为你的密码（str格式）
    res_js = test_parser(url, fname, uid, pwd)
    
