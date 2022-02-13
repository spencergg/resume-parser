ResumeSDK简历解析 - Resume Parser for Both Chinese and English CV
========

ResumeSDK是业界领先的智能简历解析、简历分析和简历评估服务厂商，将NLP/ML/DL应用于简历相关业务场景，让AI赋能招聘。
* 官网：http://www.resumesdk.com/
* DEMO：http://www.resumesdk.com/demo-parser.html
* 开发文档：http://www.resumesdk.com/docs/rs-parser.html
* 阿里云接口服务：https://market.aliyun.com/products/57124001/cmapi034316.html#sku=yuncode2831600001
除解析之外，还有如下一些功能：
* 简历查重：http://www.resumesdk.com/deduper.html
* 人岗匹配：http://www.resumesdk.com/matcher.html
* 人才搜索：http://www.resumesdk.com/searcher.html

技术特色 - Features
---

* 支持对中文和英文简历的解析；
* 支持对超过150多个简历字段的提取；
* 支持对pdf/doc/rtf/jpg等超过40种不同格式简历的解析，囊括所有常见的简历格式；
* 不仅支持对各大网站模板简历，也支持对自由格式简历的解析，基于ML/DL的技术架构拥有强大的泛化能力；
* 拥有百万量级的测试样本数据以及千万级的高准确且丰富的实体词典，基于词典加机器模型的算法，让识别效果更加精准；
* 持续7年不断的case积累和迭代优化，让最终效果精益求精；

如何使用 - Usage
---

1. 在官网上申请测试账号（uid和pwd），ResumeSDK提供最多2000次免费调用额度。参考code/下不同语言的调用实例，进行调用，获得解析结果；
2. 也可以在阿里云接口服务（不需uid和pwd，使用阿里云appcode认证方式）上面进行试用，阿里云上有调用代码实例和使用指南；

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

1. 基本信息
   1. 基本信息——基础信息
   2. 基本信息——联系方式
   3. 基本信息——期望工作
   4. 基本信息——简历信息
   5. 基本信息——头像信息
   6. 基本信息——文本内容
2. 教育经历
3. 工作经历
4. 项目经历
5. 培训经历
6. 技能列表
7. 语言技能
8. 语言证书列表
