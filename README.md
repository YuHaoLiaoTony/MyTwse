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
