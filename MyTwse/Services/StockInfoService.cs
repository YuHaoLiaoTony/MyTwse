using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyTwse.Enum;
using MyTwse.Extensions;
using MyTwse.Helpers;
using MyTwse.IRepositories;
using MyTwse.Models;
using MyTwse.Models.QueryModels;
using MyTwse.Models.ReportModels;
using MyTwse.ServiceInterface;

namespace MyTwse.Services
{
    public class StockInfoService : IStockInfoService
    {
        IInsertDateLogRepository _InsertDateLogRepository { get; set; }
        IStockInfoRepository _StockInfoRepository { get; set; }
        public StockInfoService(IInsertDateLogRepository insertDateLogRepository, IStockInfoRepository stockInfoRepository)
        {
            _InsertDateLogRepository = insertDateLogRepository;
            _StockInfoRepository = stockInfoRepository;
        }

        public List<StockInfo> GetStockPERank(DateTime date, int count)
        {
            Task<bool> isHoliday = date.IsHoliday();

            isHoliday.Wait();

            if (isHoliday.Result)
            {
                throw new MyTwseException(MyTwseExceptionEnum.BadRequestIsHoliday, $"{date.ToString("yyyy-MM-dd")}");
            }

            CreateStockInfoData(date);

            var result = _StockInfoRepository.GetPagedListOrderBy(1, count, e => e.Date == date.Date && e.PE.HasValue, e => e.PE);

            if(result.Any() == false)
            {
                throw new MyTwseException(MyTwseExceptionEnum.NotFount, $"{date.ToString("yyyy-MM-dd")}");
            }

            return result;
        }
        public List<StockInfo> GetStockInfos()
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            DateTime endDate = now.Date;
            DateTime startDate = endDate.AddDays(-5);

            CreateStockInfoData(startDate, endDate);

            return _StockInfoRepository.GetPagedListOrderBy(1, 100, e => true, e => e.Date);
        }
        public List<StockInfo> GetStockInfoByRecent(string stockCode, int days)
        {
            if (days <= 0)
            {
                throw new MyTwseException(MyTwseExceptionEnum.BadRequest,$"{nameof(days)}必須大於等於 1");
            }
            if (stockCode.IsNumber() == false)
            {
                throw new MyTwseException(MyTwseExceptionEnum.BadRequestIsNotNumber);
            }
            days -= 1;
            DateTime now = DateTime.UtcNow.AddHours(8);

            DateTime endDate = now.Date;
            DateTime startDate = endDate.AddDays(-days);

            CreateStockInfoData(startDate, endDate);

            return _StockInfoRepository.GetListBy(e => e.Code == stockCode && e.Date >= startDate && e.Date <= endDate);
        }
        /// <summary>
        /// 取得股票殖利率提升連續最大天數
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public List<YieldRateIncreaseＭaxDaysReportModel> GetYieldRateIncreaseＭaxDays(YieldRateIncreaseＭaxDaysQueryModel queryModel)
        {
            if (queryModel.StartDate > queryModel.EndDate)
            {
                throw new MyTwseException(MyTwseExceptionEnum.BadRequest, "開始時間不得大於結束時間");
            }
            CreateStockInfoData(queryModel.StartDate.Value, queryModel.EndDate.Value);
            return _StockInfoRepository.GetYieldRateIncreaseＭaxDays(queryModel);
        }
        /// <summary>
        /// 確認需求範圍是否有未取得的資料
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void CreateStockInfoData(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            string url = "https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&selectType=ALL";

            int insertDateLogsType = (int)InsertDateLogsTypeEnum.StockInfo;
            var insertDateLogDic =
               _InsertDateLogRepository
               .GetListBy(e => e.Date >= startDate && e.Date <= endDate && e.Type == insertDateLogsType)
               .ToDictionary(e => e.Date, e => e);
                        
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                List<StockInfo> stockInfos = new List<StockInfo>();
                List<InsertDateLog> insertDateLogs = new List<InsertDateLog>();
                if (insertDateLogDic.ContainsKey(date))
                {
                    continue;
                }
                //一個日期呼叫一次API
                StockInfoJsonModel result = null;

                try
                {
                    result = RestRequestHelper.Request(url)
                    .Get(e => e
                        .AddParameter("date", date.ToString("yyyyMMdd"))
                        )
                    .Response<StockInfoJsonModel>();

                    //API 一直打會被鎖,故隨機延遲300~500毫秒
                    Task.Delay(GetRandom(300, 500)).Wait();
                }
                catch(Exception ex)
                {
                    //錯誤就跳出迴圈
                    break;
                }
                if (result == null)
                {
                    //不紀錄該日期已經取得資料
                    continue;
                }
                //紀錄 API日期
                insertDateLogs.Add(new InsertDateLog
                {
                    Type = insertDateLogsType,
                    Date = date,
                    CreateTime = now
                });
                //為 null 可能是假日或沒開市所以查無資料
                if (result.Data != null)
                {
                    foreach (var item in result.Data)
                    {
                        /*["證券代號","證券名稱","殖利率(%)","股利年度","本益比","股價淨值比","財報年/季"]*/
                        stockInfos.Add(new StockInfo
                        {
                            Code = item[0]?.ToString(),
                            Name = item[1]?.ToString(),
                            YieldRate = item[2]?.ToString().TryToDecimal(),
                            DividendYear = (item[3]?.ToString()).TryToInt().GetValueOrDefault(),
                            PE = item[4]?.ToString().TryToDecimal(),
                            PB = item[5]?.ToString().TryToDecimal(),
                            FinancialReport = item[6]?.ToString(),
                            Date = date
                        });
                    }
                }
                
                _StockInfoRepository.Create(stockInfos, insertDateLogs);
            }
           
        }
        private int GetRandom(int min, int max)
        {
            Random random = new Random();//亂數種子

            return random.Next(min, max);//回傳0-99的亂數;
        }
        /// <summary>
        /// 確認需求範圍是否有未取得的資料
        /// </summary>
        /// <param name="date"></param>
        private void CreateStockInfoData(DateTime date)
        {
            CreateStockInfoData(date, date);
        }
    }
}
