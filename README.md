# TwseAPI 
- 這是一個從證交所取得股票資訊的API
- 資料來源 [個股日本益比、殖利率及股價淨值比](https://www.twse.com.tw/zh/page/trading/exchange/BWIBBU_d.html)

## 取得前 100筆股票資料
- 若資料庫沒有任何資料會呼叫證交所的API取得近５日資料
- 主要用來測試使用
```http
GET /StockInfo
```

### Responses
```json
[
  {
    "id": 315586,
    "name": "辛耘",
    "code": "3583",
    "yieldRate": 3.93,
    "dividendYear": 106,
    "pe": 10.41,
    "pb": 1.74,
    "financialReport": "107/3",
    "date": "2019-01-02T00:00:00"
  },
  {
    "id": 315587,
    "name": "通嘉",
    "code": "3588",
    "yieldRate": 2.43,
    "dividendYear": 106,
    "pe": 26.41,
    "pb": 0.74,
    "financialReport": "107/3",
    "date": "2019-01-02T00:00:00"
  }
]
```
## 指定特定日期，顯示當天本益比前n名
```http
/StockInfo/PE/Rank
```
| Parameter | Type | Description |
| :--- | :--- | :--- |
| `date` | `datetime` | **Required**. 日期 ex. 2021/04/09 (日期不能為假日) |
| `count` | `number` | **Required**. 筆數 ex. 100|

### Responses
```json
[
    {
        "id": 127616,
        "name": "秋雨",
        "code": "9929",
        "yieldRate": 3.10,
        "dividendYear": 109,
        "pe": 1.53,
        "pb": 1.23,
        "financialReport": "109/4",
        "date": "2021-04-09T00:00:00"
    },
    {
        "id": 128125,
        "name": "陽明",
        "code": "2609",
        "yieldRate": 0.00,
        "dividendYear": 108,
        "pe": 11.31,
        "pb": 3.80,
        "financialReport": "109/4",
        "date": "2021-04-09T00:00:00"
    }
]
```
