using MyTwse.Models;
using MyTwse.Models.QueryModels;
using MyTwse.Models.ReportModels;
using System;
using System.Collections.Generic;

namespace MyTwse.ServiceInterface
{
    public interface IStockInfoService
    {
        /// <summary>
        /// 取得股票近幾天資料
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        List<StockInfo> GetStockInfoByRecent(string stockCode, int days);
        /// <summary>
        /// 取得股票資訊
        /// </summary>
        /// <returns></returns>
        List<StockInfo> GetStockInfos();
        /// <summary>
        /// 本益比排行榜
        /// </summary>
        /// <param name="date"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StockInfo> GetStockPERank(DateTime date, int count);
        /// <summary>
        /// 取得股票殖利率提升連續最大天數
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        List<YieldRateIncreaseＭaxDaysReportModel> GetYieldRateIncreaseＭaxDays(YieldRateIncreaseＭaxDaysQueryModel queryModel);
        /// <summary>
        /// 建立股票資料
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        void CreateStockInfoData(DateTime startDate, DateTime endDate);
    }
}