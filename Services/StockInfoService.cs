using MyTwse.Extensions;
using MyTwse.Helpers;
using MyTwse.Models;
using MyTwse.Models.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTwse.Services
{
    public interface IStockInfoService
    {
        void GetStockInfoByRecent(string stockCode, int days);
    }
    public enum InsertDateLogsTypeEnum
    {
        StockInfo = 1
    }
    public class StockInfoService : IStockInfoService
    {
        public void GetStockInfoByRecent(string stockCode, int days)
        {
            string url = "https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&selectType=ALL";
            DateTime now = DateTime.UtcNow.AddHours(8);
            DateTime nowDate = now.Date;
            var db = new TwseStockContext();
            int insertDateLogsType = (int)InsertDateLogsTypeEnum.StockInfo;
            var insertDateLogs =
                db.InsertDateLog
                .Where(e => e.Date >= nowDate.AddDays(-days) && e.Date <= nowDate && e.Type == insertDateLogsType)
                .ToDictionary(e => e.Date, e => e);

            for (DateTime date = nowDate; date > nowDate.AddDays(-days); date = date.AddDays(-1))
            {
                if(insertDateLogs.ContainsKey(date))
                {
                    continue;
                }

                var result = RestRequestHelper.Request(url)
                    .Get(e => e
                        .AddParameter("date", nowDate.ToString("yyyyMMdd"))
                        ).Response<StockInfoJsonModel>();

                db.InsertDateLog.Add(new InsertDateLog
                {
                    Type = insertDateLogsType,
                    Date = date,
                    CreateTime = now
                });
                
                foreach (var item in result.Data)
                {
                    /*["證券代號","證券名稱","殖利率(%)","股利年度","本益比","股價淨值比","財報年/季"]*/
                    db.StockInfo.Add(new StockInfo
                    {
                        Code = item[0]?.ToString(),
                        Name = item[1]?.ToString(),
                        YieldRate = item[2]?.ToString(),
                        DividendYear = (item[3]?.ToString()).TryToInt().GetValueOrDefault(),
                        PE = item[4]?.ToString(),
                        PB = item[5]?.ToString(),
                        FinancialReport = item[6]?.ToString(),
                        Date = date
                    });
                }
            }
            db.SaveChanges();
            //data source=localhost\SQLEXPRESS01;initial catalog=TwseStock;integrated security=true;

        }

    }
}
