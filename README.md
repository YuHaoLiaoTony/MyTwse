[TOC]

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
    "Code": 200,
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

