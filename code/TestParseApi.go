package main

import (
    "bytes"
    "fmt"
    "log"
    "net/http"
    "os"
    "encoding/base64"
    "encoding/json"
    "bufio"
    "io/ioutil"
)

type Input struct {
    File_name string `json:"file_name"`
    File_cont string `json:"file_cont"`
    Need_avatar int `json:"need_avatar"`
}

// Creates http request
func createRequest(url string, uid int, pwd string, fname string) (*http.Request, error) {
    file, err := os.Open(fname)
    if err != nil {
        return nil, err
    }
    defer file.Close()

    // Read entire file into byte slice.
    reader := bufio.NewReader(file)
    content, _ := ioutil.ReadAll(reader)

    // Create Request
    file_cont := base64.StdEncoding.EncodeToString(content)
    input := Input{File_name: fname, File_cont: file_cont, Need_avatar: 0}
    jsonValue, _ := json.Marshal(input)
    req, err := http.NewRequest("POST", url, bytes.NewBuffer(jsonValue))

    // Set headers
    req.Header.Set("Content-Type", "application/json")
    uid_str := fmt.Sprintf("%d", uid)
    req.Header.Set("uid", uid_str)
    req.Header.Set("pwd", pwd)
    return req, err
}

func testParser(url string, uid int, pwd string, fname string) {
    request, err := createRequest(url, uid, pwd, fname)
    if err != nil {
        log.Fatal(err)
    }

    client := &http.Client{}
    resp, err := client.Do(request)
    fmt.Println("http status code: ", resp.StatusCode)
    if err != nil {
        log.Fatal(err)
    } else {
        body := &bytes.Buffer{}
        _, err := body.ReadFrom(resp.Body)
        if err != nil {
            log.Fatal(err)
        }
        resp.Body.Close()
        fmt.Println(body)
    }
}

func main() {
    url := "http://www.resumesdk.com/api/parse"      // 接口地址，也可以用https
    uid := 123456    // 替换为你的用户名（int格式）
    pwd  := "123abc" // 替换为你的密码（str格式）
    fname := "D:/resumeSDK/resume.docx"  //替换为您的简历
    testParser(url, uid, pwd, fname) 
}
