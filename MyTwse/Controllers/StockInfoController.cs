using Microsoft.AspNetCore.Mvc;
using MyTwse.Models;
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
        [Route("StockInfo/Recent")]
        public List<StockInfo> GetStockInfoByRecent(string stockCode, int days = 5)
        {
            return _StockInfoService.GetStockInfoByRecent(stockCode, days);
        }

        [Route("StockInfo/PE/Rank")]
        public List<StockInfo> GetStockPERank(DateTime date,int count = 100)
        {
            return _StockInfoService.GetStockPERank(date, count);
        }
        [HttpPost]
        [Route("StockInfo")]
        public void CreateStockInfoData(CreateStockViewModel model)
        {
            _StockInfoService.CreateStockInfoData(model.StartDate.Value, model.EndDate.Value);
        }
    }

}
