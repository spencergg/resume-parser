ResumeSDK简历解析 - Resume Parser for Both Chinese and English CV
========

ResumeSDK是业界领先的智能简历解析、简历分析和简历评估服务厂商，将NLP/ML/DL应用于简历相关业务场景，让AI赋能招聘。(注：ResumeSDK简历解析为商业产品，可以提供免费调用，非开源项目）
* 官网：https://www.resumesdk.com/
* DEMO：https://www.resumesdk.com/demo-parser.html
* 接口文档：https://www.resumesdk.com/docs/rs-parser.html
* 调用示例：https://www.resumesdk.com/docs/rs-parser.html#allDemo
* 阿里云接口服务：https://market.aliyun.com/products/57124001/cmapi034316.html
* 腾讯云接口服务：https://market.cloud.tencent.com/products/32878

除解析之外，还有如下一些功能：
* 简历画像：https://www.resumesdk.com/profiler.html
* 简历查重：https://www.resumesdk.com/deduper.html
* 人岗匹配：https://www.resumesdk.com/matcher.html
* 人才推荐：https://www.resumesdk.com/recer.html

技术特色 - Features
---

* 支持对中文和英文简历的解析；
* 支持对超过170多个简历字段的提取；
* 支持对pdf/doc/rtf/jpg等超过40种不同格式简历的解析，囊括所有常见的简历格式；
* 不仅支持对各大网站模板简历，也支持对自由格式简历的解析，基于ML/DL的技术架构拥有强大的泛化能力；
* 拥有百万量级的测试样本数据以及千万级的高准确且丰富的实体词典，基于词典加机器模型的算法，让识别效果更加精准；
* 持续10年不断的case积累和迭代优化，让最终效果精益求精；

如何使用 - Usage
---

1. 在官网上申请测试账号（uid和pwd），ResumeSDK提供最多2000次免费调用额度。参考code/下不同语言的调用示例，进行调用，获得解析结果；
2. 也可以在阿里云接口服务（不需uid和pwd，使用阿里云appcode认证方式）上面进行试用，官网上有调用代码实例和使用指南；

python代码实例：
```python
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
```

解析结果
---

https://www.resumesdk.com/docs/rs-parser.html#resumeStruct

1. 基本信息
   1. 基本信息——基础信息
   2. 基本信息——联系方式
   3. 基本信息——期望工作
   4. 基本信息——简历信息
   5. 基本信息——头像信息
   6. 基本信息——文本内容
2. 教育经历
3. 工作经历及实习经历
4. 社会及学校实践经历
5. 项目经历
6. 培训经历
7. 技能列表
8. 语言技能
9. 语言证书列表
10. 证书及奖项
