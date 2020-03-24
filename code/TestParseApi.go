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
    Uid int `json:"uid"`
    Pwd string `json:"pwd"`
    File_name string `json:"file_name"`
    File_cont string `json:"file_cont"`
}

// Creates http request
func createRequest(url string, uid int, pwd string, path string) (*http.Request, error) {
    file, err := os.Open(path)
    if err != nil {
        return nil, err
    }
    defer file.Close()

    // Read entire file into byte slice.
    reader := bufio.NewReader(file)
    content, _ := ioutil.ReadAll(reader)

    // Encode as base64.
    file_cont := base64.StdEncoding.EncodeToString(content)
    input := Input{Uid: uid, Pwd: pwd, File_name: path, File_cont: file_cont}
    jsonValue, err := json.Marshal(input)
    if err != nil {
        fmt.Println("Umarshal failed:", err)
        return nil, err
    }
    req, err := http.NewRequest("POST", url, bytes.NewBuffer(jsonValue))
    req.Header.Set("Content-Type", "application/json")
    return req, err
}

func test_parser(url string, uid int, pwd string, file_path string) {
    request, err := createRequest(url, uid, pwd, file_path)
    if err != nil {
        log.Fatal(err)
    }

    client := &http.Client{}
    resp, err := client.Do(request)
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
    url := "http://www.resumesdk.com/api/parse"
    uid := 123456 	// 替换为你的用户名（int格式）
    pwd	:= "123abc"	// 替换为你的密码（str格式）
    fname := "D:/test_input/resume.docx"	// 替换为你的简历文件名，确保后缀名正确
    test_parser(url, uid, pwd, fname) 
}
