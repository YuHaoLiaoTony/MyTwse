using MyTwse.Models;
using System;
using System.Collections.Generic;

namespace MyTwse.ServiceInterface
{
    public interface IStockInfoService
    {
        List<StockInfo> GetStockInfoByRecent(string stockCode, int days);
        List<StockInfo> GetStockInfos();
        List<StockInfo> GetStockPERank(DateTime date, int count);
    }
}