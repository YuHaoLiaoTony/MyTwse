using Microsoft.AspNetCore.Mvc;
using MyTwse.Models;
using MyTwse.ServiceInterface;
using System.Collections.Generic;

namespace MyTwse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockInfoController : Controller
    {
        IStockInfoService _StockInfoService { get; set; }
        public StockInfoController(IStockInfoService stockInfoService)
        {
            _StockInfoService = stockInfoService;
        }
        public List<StockInfo> Index(string stockCode, int days = 5)
        {
            return _StockInfoService.GetStockInfoByRecent(stockCode, days);
        }

    }
}
