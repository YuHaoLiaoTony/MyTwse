using MyTwse.Models;
using MyTwse.Models.QueryModels;
using MyTwse.Models.ReportModels;
using MyTwse.Repositories;
using System.Collections.Generic;

namespace MyTwse.IRepositories
{
    public interface IStockInfoRepository : IBaseRepository<StockInfo>
    {
        void Create(List<StockInfo> stockInfos, List<InsertDateLog> insertDateLogs);
        /// <summary>
        /// 取得股票殖利率提升連續最大天數
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        List<YieldRateIncreaseＭaxDaysReportModel> GetYieldRateIncreaseＭaxDays(YieldRateIncreaseＭaxDaysQueryModel queryModel);
    }
}
