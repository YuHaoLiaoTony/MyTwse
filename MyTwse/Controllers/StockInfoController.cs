using Microsoft.AspNetCore.Mvc;
using MyTwse.Models;
using MyTwse.Models.QueryModels;
using MyTwse.Models.ReportModels;
using MyTwse.Models.ViewModels;
using MyTwse.ServiceInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyTwse.Controllers
{
    [ApiController]
    public class StockInfoController : Controller
    {
        IStockInfoService _StockInfoService { get; set; }
        public StockInfoController(IStockInfoService stockInfoService)
        {
            _StockInfoService = stockInfoService;
        }
        [Route("StockInfo")]
        public List<StockInfo> Index()
        {
            return _StockInfoService.GetStockInfos();
        }
        /// <summary>
        /// 依照證券代號 搜尋最近n天的資料
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        [Route("StockInfo/Recent")]
        public List<StockInfo> GetStockInfoByRecent(string stockCode, int days = 5)
        {
            return _StockInfoService.GetStockInfoByRecent(stockCode, days);
        }
        /// <summary>
        /// 指定特定日期 顯示當天本益比前n名
        /// </summary>
        /// <param name="date"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Route("StockInfo/PE/Rank")]
        public List<StockInfo> GetStockPERank(DateTime date,int count = 100)
        {
            return _StockInfoService.GetStockPERank(date, count);
        }
        /// <summary>
        /// 新增股票資料
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Route("StockInfo")]
        public void CreateStockInfoData(CreateStockViewModel model)
        { 
            _StockInfoService.CreateStockInfoData(model.StartDate.Value, model.EndDate.Value);
        }
        [HttpGet]
        [Route("StockInfo/YieldRateIncreaseＭaxDays")]
        public List<YieldRateIncreaseＭaxDaysReportModel> GetYieldRateIncreaseＭaxDays([FromQuery]YieldRateIncreaseＭaxDaysQueryModel queryModel)
        {
            return _StockInfoService.GetYieldRateIncreaseＭaxDays(queryModel);
        }
    }

}
