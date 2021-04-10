using MyTwse.Models;
using System;
using System.Collections.Generic;

namespace MyTwse.ServiceInterface
{
    public interface IStockInfoService
    {
        List<StockInfo> GetStockInfoByRecent(string stockCode, int days);
        List<StockInfo> GetStockInfos();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StockInfo> GetStockPERank(DateTime date, int count);
        /// <summary>
        /// 建立股票資料
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        void CreateStockInfoData(DateTime startDate, DateTime endDate);
    }
}