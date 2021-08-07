# Take Home Engineering Challenge
  
白版題能否真實反應工程師的程度一直以來都存在爭議  
隨著Leetcode之類的題庫出現  
這類題目的難度也越拉越高  
造成【面試造火箭 工作擰螺絲】的現象產生  
對實際工作上會遇到的挑戰內容有落差是面試雙方都不希望發生的  
  
我們期待改由透過實際撰寫一個更真實的專案來了解您的技術實力  
  
## Guidelines
  
- 團隊目前主要使用的語言是C#，希望您對這門語言有一些基本的了解  
之前沒寫過沒有關係，這個專案可以當成一個好的開頭  
在**實際工作之前應該先試試以後會很常相處的東西，確保自己並不討厭C#的各種特性**  
  
-   微軟這幾年很明顯地擁抱開源社區 .Net的未來發展也一樣  
希望您知道如何使用open source的資源  
請將這份任務當成一個 **open source project**  
**使用git做版控，撰寫良好的README.md告訴別人該怎麼使用您的作品**  
  
-   團隊重視程式碼品質並且目前是使用StyleCop檢查CodingStyle  
希望您的程式碼也有良好的**可讀性**  
ps. 對 C# 這門語言[MSDN](https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)是一份很好的參考資料  
  
-   比起期待工程師永遠不會狀況不佳，我們更相信穩定的測試案例  
希望您也認同撰寫單元測試的重要性，並且**實際撰寫單元測試**  
  
## The Problem  
CMoney的產品與服務大量涉及了對證券的蒐集、處理、展示  
您的任務是利用 `台灣證券交易所` 提供的 [個股日本益比、殖利率及股價淨值比](https://www.twse.com.tw/zh/page/trading/exchange/BWIBBU_d.html) 實作  
* 依照`證券代號` 搜尋最近n天的資料  
* 指定`特定日期` 顯示當天`本益比`前n名  
* 指定`日期範圍`、`證券代號` 顯示這段時間內`殖利率` 為嚴格遞增的最長天數並顯示開始、結束日期  
  
如何展示上述需求沒有任何限制  
- 如果您擅長後端可以設計一套web API 回傳結果  
- 如果您習慣使用Pipeline工作可以實作一個CLI Tool  
- 如果您除了會Coding還對資料視覺化有心得可以實作web frontend展示結果  
- 其他您覺得自己擅長的方式  
  
**其他要求**   
- 資料必須允許在線新增  
相信您也發現了證交所提供的檔案為每日一個獨立的csv檔  
實際分析資料時會用到不止一天  
請考慮如何匯入資料  
- 必須使用git開發  
- 必須有文件  
- 必須有單元測試  
  
---
這份任務是為了讓我們更好的了解您  
歡迎在文件中描述您是如何設計的、用了哪些技巧，做了哪些trade-off  
  
可以直接在此Gitlab註冊一個帳號建立一個倉庫  
或是在任何您習慣的地方開發(Github、Bitbucket)  
  
請務必在收到題目後七天內將作品連結或壓縮檔透過email回復  

---
<!-- TOC -->

- [TwseAPI](#twseapi)
    - [API通用格式](#api%E9%80%9A%E7%94%A8%E6%A0%BC%E5%BC%8F)
    - [取得前 100筆股票資料](#%E5%8F%96%E5%BE%97%E5%89%8D-100%E7%AD%86%E8%82%A1%E7%A5%A8%E8%B3%87%E6%96%99)
        - [Responses](#responses)
    - [依照證券代號 搜尋最近n天的資料](#%E4%BE%9D%E7%85%A7%E8%AD%89%E5%88%B8%E4%BB%A3%E8%99%9F-%E6%90%9C%E5%B0%8B%E6%9C%80%E8%BF%91n%E5%A4%A9%E7%9A%84%E8%B3%87%E6%96%99)
        - [Responses](#responses)
    - [顯示指定日期本益比前n名](#%E9%A1%AF%E7%A4%BA%E6%8C%87%E5%AE%9A%E6%97%A5%E6%9C%9F%E6%9C%AC%E7%9B%8A%E6%AF%94%E5%89%8Dn%E5%90%8D)
        - [Responses](#responses)
    - [殖利率嚴格遞增的最長天數](#%E6%AE%96%E5%88%A9%E7%8E%87%E5%9A%B4%E6%A0%BC%E9%81%9E%E5%A2%9E%E7%9A%84%E6%9C%80%E9%95%B7%E5%A4%A9%E6%95%B8)
    - [新增個股資訊](#%E6%96%B0%E5%A2%9E%E5%80%8B%E8%82%A1%E8%B3%87%E8%A8%8A)
        - [Request](#request)
        - [Responses](#responses)

<!-- /TOC -->

# TwseAPI

- 這是一個從證交所取得股票資訊的API
- 資料來源 [個股日本益比、殖利率及股價淨值比](https://www.twse.com.tw/zh/page/trading/exchange/BWIBBU_d.html)

## API通用格式

- 其中 `Data` 的格式會依照實際情況改變
- 範例如下

1. Data 為 Array

```json
{
    "Data": [
        {
            "Id": 315586,
            "Name": "辛耘",
            "Code": "3583",
            "YieldRate": 3.93,
            "DividendYear": 106,
            "PE": 10.41,
            "PB": 1.74,
            "FinancialReport": "107/3",
            "Date": "2019-01-02T00:00:00"
        },
        {
            "Id": 315587,
            "Name": "通嘉",
            "Code": "3588",
            "YieldRate": 2.43,
            "DividendYear": 106,
            "PE": 26.41,
            "PB": 0.74,
            "FinancialReport": "107/3",
            "Date": "2019-01-02T00:00:00"
        }
    ],
    "Code": 200,
    "Message": ""
}
```

2. Data 為 Object

```json
{
    "Data": {
        "IsSuccess": true
    },
    "Code": 200,
    "Message": ""
}
```
## 取得前 100筆股票資料

- 若資料庫沒有任何資料會呼叫證交所的API取得近５日資料
- 主要用來測試使用
```http
[GET] /StockInfo
```

### Responses

```json
{
    "Data": [
        {
            "Id": 315586,
            "Name": "辛耘",
            "Code": "3583",
            "YieldRate": 3.93,
            "DividendYear": 106,
            "PE": 10.41,
            "PB": 1.74,
            "FinancialReport": "107/3",
            "Date": "2019-01-02T00:00:00"
        },
        {
            "Id": 315587,
            "Name": "通嘉",
            "Code": "3588",
            "YieldRate": 2.43,
            "DividendYear": 106,
            "PE": 26.41,
            "PB": 0.74,
            "FinancialReport": "107/3",
            "Date": "2019-01-02T00:00:00"
        }
    ],
    "Code": 200,
    "Message": ""
}
```
## 依照證券代號 搜尋最近n天的資料

```http
[Get] /StockInfo/Recent
```
| Parameter | Type | Description |
| :--- | :--- | :--- |
| stockCode | string | **Required**. 2852 |
| days | `number` | **Required**. 筆數 ex. 100|

### Responses
```json
{
    "Data": [
        {
            "Id": 1411,
            "Name": "第一保",
            "Code": "2852",
            "YieldRate": 2.68,
            "DividendYear": 109,
            "PE": 25.59,
            "PB": 0.58,
            "FinancialReport": "109/4",
            "Date": "2021-04-08T00:00:00"
        },
        {
            "Id": 127546,
            "Name": "第一保",
            "Code": "2852",
            "YieldRate": 2.67,
            "DividendYear": 109,
            "PE": 25.69,
            "PB": 0.58,
            "FinancialReport": "109/4",
            "Date": "2021-04-09T00:00:00"
        }
    ],
    "Code": 200,
    "Message": ""
}
```
## 顯示指定日期本益比前n名

```http
[Get] /StockInfo/PE/Rank
```
| Parameter | Type | Description |
| :--- | :--- | :--- |
| `date` | `datetime` | **Required**. 日期 ex. 2021/04/09 (日期不能為假日) |
| `count` | `number` | **Required**. 筆數 ex. 100|

### Responses

```json
{
    "Data": [
        {
            "Id": 127616,
            "Name": "秋雨",
            "Code": "9929",
            "YieldRate": 3.10,
            "DividendYear": 109,
            "PE": 1.53,
            "PB": 1.23,
            "FinancialReport": "109/4",
            "Date": "2021-04-09T00:00:00"
        },
        {
            "Id": 127911,
            "Name": "達新",
            "Code": "1315",
            "YieldRate": 10.60,
            "DividendYear": 109,
            "PE": 1.86,
            "PB": 0.83,
            "FinancialReport": "109/4",
            "Date": "2021-04-09T00:00:00"
        }
    ],
    "Code": 200,
    "Message": ""
}
```

## 殖利率嚴格遞增的最長天數

```http
[GET] StockInfo/YieldRateIncreaseＭaxDays
```

| Parameter   | Type       | IsRequired | Description                          |
| :---------- | :--------- | ---------- | :----------------------------------- |
| `startDate` | `datetime` | True       | 日期 ex. 2021/04/09 (日期不能為假日) |
| `endDate`   | `number`   | True       | 筆數 ex. 100                         |
| `Code`      | `string`   | True       | 股票代碼                             |

```json
{
    "Data": [
        {
            "MinDate": "2020-07-21T00:00:00",
            "MaxDate": "2020-07-27T00:00:00",
            "Days": 5
        }
    ],
    "Code": 200,z
    "Message": ""
}
```



## 新增個股資訊

```http
[Post] StockInfo
```

### Request

```json
{
    "StartDate":"2021-04-01",
    "endDate":"2021-04-11"
}
```

### Responses

```json
{
    "Data": {
        "IsSuccess": true
    },
    "Code": 200,
    "Message": ""
}
```



# 程式碼反饋


- 拿`免费节假日API` 來判斷台灣的國定假日應該是不準的。有需要判斷建議抓 [政府行政機關辦公日曆表](https://data.ntpc.gov.tw/openapi/swagger-ui/index.html?configUrl=%2Fopenapi%2Fswagger%2Fconfig&urls.primaryName=%E6%96%B0%E5%8C%97%E5%B8%82%E6%94%BF%E5%BA%9C%E4%BA%BA%E4%BA%8B%E8%99%95(20)#/JSON/get_308DCD75_6434_45BC_A95F_584DA4FED251_json)來處理  
- `StyleCop.Cache`、`.user` 之類的檔案不應該 Commit 上版控。 建議修改更完整的 [.gitignore](https://github.com/github/gitignore/blob/master/VisualStudio.gitignore) 來使用  
- 單元測試未包含查詢指令是否正確的測試，比較期待看到使用 `UseInMemoryDatabase` 設計好 TestCase 來驗證複雜 SQL 的正確性
