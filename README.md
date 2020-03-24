ResumeSDK简历解析 - Resume Parser for Both Chinese and English CV
========

ResumeSDK是业界领先的智能简历解析、简历分析和简历评估服务厂商，将NLP/ML/DL应用于简历相关业务场景，让AI赋能招聘。
* 官网：http://www.resumesdk.com/
* DEMO：http://www.resumesdk.com/demo-parser.html
* 开发文档：http://www.resumesdk.com/docs/rs-parser.html

技术特色 - Features
---

* 支持对超过140多个简历字段的提取；
* 支持对pdf/doc/rtf/jpg等超过40种不同格式简历的解析，囊括所有常见的简历格式；
* 不仅支持对各大网站模板简历，也支持对自由格式简历的解析；
* 拥有百万量级的测试样本数据以及千万级的高准确且丰富的实体词典，基于词典加机器模型的算法，让识别效果更加精准；

如何使用 - Usage
---

1. 在官网申请测试账号（uid和pwd），ResumeSDK提供最多2000次免费调用额度；
2. 参考code/下不同语言的调用实例，进行调用，获得解析结果；

python代码实例：
```python
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
```

解析结果
---

http://www.resumesdk.com/docs/rs-parser.html#resumeStruct

1 基本信息
1.1 基本信息——基础信息
1.2 基本信息——联系方式
1.3 基本信息——期望工作
1.4 基本信息——简历信息
1.5 基本信息——头像信息
1.6 基本信息——文本内容
2 教育经历
3 工作经历
4 项目经历
5 培训经历
6 技能列表
7 语言技能
8 语言证书列表
