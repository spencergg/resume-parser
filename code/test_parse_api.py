#coding: utf-8

import sys
import base64
import requests
import json

def test_parser(url, fname, uid, pwd):
    # 读取文件内容，构造请求
    cont = open(fname, 'rb').read()
    base_cont = base64.b64encode(cont)
    base_cont = base_cont.decode('utf-8') if sys.version.startswith('3') else base_cont     #兼容python2与python3
    data = {'uid': uid,
            'pwd': str(pwd),
            'file_name': fname,
            'file_cont': base_cont,
            }
    
    # 发送请求
    res = requests.post(url, data=json.dumps(data))
    
    # 解析结果
    res_js = json.loads(res.text)
    print('result:\n%s\n'%(json.dumps(res_js, indent=2, ensure_ascii=False)))
    
    if 'result' in res_js:
        print('name: %s'%(res_js['result'].get('name', 'None')))

    return res_js
    
if __name__ == '__main__' :
    url = 'http://www.resumesdk.com/api/parse'
    fname = u'D:/test_input/resume.docx'  # 替换为你的简历文件名，确保后缀名正确
    uid = 123456   # 替换为你的用户名（int格式）
    pwd = '123abc'  # 替换为你的密码（str格式）
    res_js = test_parser(url, fname, uid, pwd)
    