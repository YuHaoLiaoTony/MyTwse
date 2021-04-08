using MyTwse.Models;
using System.Collections.Generic;

namespace MyTwse.ServiceInterface
{
    public interface IStockInfoService
    {
        List<StockInfo> GetStockInfoByRecent(string stockCode, int days);
    }
}