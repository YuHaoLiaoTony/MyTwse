using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            _StockInfoService.GetStockInfoByRecent("", 1);
            return View();
        }
    }
}
