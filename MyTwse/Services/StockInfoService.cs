﻿using MyTwse.Enum;
using MyTwse.Extensions;
using MyTwse.Helpers;
using MyTwse.IRepositories;
using MyTwse.Models;
using MyTwse.Models.ReportModel;
using MyTwse.Repositories;
using MyTwse.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;

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
            CreateStockInfoData(date);
            return _StockInfoRepository.GetPagedListOrderBy(1, count, e => e.Date == date.Date && e.PE.HasValue, e => e.PE);
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
            days -= 1;
            DateTime now = DateTime.UtcNow.AddHours(8);

            DateTime endDate = now.Date;
            DateTime startDate = endDate.AddDays(-days);

            CreateStockInfoData(startDate, endDate);

            return _StockInfoRepository.GetListBy(e => e.Code == stockCode && e.Date >= startDate && e.Date <= endDate);
        }
        private void CreateStockInfoData(DateTime date)
        {
            CreateStockInfoData(date, date);
        }
        private void CreateStockInfoData(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            string url = "https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&selectType=ALL";

            int insertDateLogsType = (int)InsertDateLogsTypeEnum.StockInfo;
            var insertDateLogDic =
               _InsertDateLogRepository
               .GetListBy(e => e.Date >= startDate && e.Date <= endDate && e.Type == insertDateLogsType)
               .ToDictionary(e => e.Date, e => e);

            List<InsertDateLog> insertDateLogs = new List<InsertDateLog>();
            List<StockInfo> stockInfos = new List<StockInfo>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (insertDateLogDic.ContainsKey(date))
                {
                    continue;
                }
                //一個日期呼叫一次API
                var result = RestRequestHelper.Request(url)
                    .Get(e => e
                        .AddParameter("date", date.ToString("yyyyMMdd"))
                        ).Response<StockInfoJsonModel>();

                //紀錄 API日期
                insertDateLogs.Add(new InsertDateLog
                {
                    Type = insertDateLogsType,
                    Date = date,
                    CreateTime = now
                });

                //為 null 可能是假日或沒開市所以查無資料
                if (result.Data == null)
                {
                    continue;
                }

                foreach (var item in result.Data)
                {
                    /*["證券代號","證券名稱","殖利率(%)","股利年度","本益比","股價淨值比","財報年/季"]*/
                    stockInfos.Add(new StockInfo
                    {
                        Code = item[0]?.ToString(),
                        Name = item[1]?.ToString(),
                        YieldRate = item[2]?.ToString(),
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
}
