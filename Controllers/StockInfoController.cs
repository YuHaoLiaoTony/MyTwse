using Microsoft.AspNetCore.Mvc;
using MyTwse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void Index()
        {
            _StockInfoService.GetStockInfoByRecent("", 5);
        }
    }
}
