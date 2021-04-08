using Microsoft.AspNetCore.Mvc;
using MyTwse.Models;
using MyTwse.ServiceInterface;
using System;
using System.Collections.Generic;

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

    }
}
