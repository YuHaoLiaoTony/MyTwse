using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTwse;
using MyTwse.ServiceInterface;
using MyTwse.Services;

namespace MyTwseTest
{
    [TestClass]
    public class GetStockInfoByRecentTest
    {
        static IWebHost _webHost = null;
        static T GetService<T>()
        {
            var scope = _webHost.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
        }

        [TestMethod]
        public void StockCode不是數字()
        {
            var ctx = GetService<IStockInfoService>();
            
            Assert.ThrowsException<MyTwseException>(()=>
            {
                ctx.GetStockInfoByRecent(stockCode:"1234KY",days: 1);
            }, "輸入值只能是數字");
        }

        [TestMethod]
        public void Days小於等於0()
        {
            var ctx = GetService<IStockInfoService>();

            Assert.ThrowsException<MyTwseException>(() =>
            {
                ctx.GetStockInfoByRecent(stockCode: "2852", days: 0);
            }, $"days必須大於等於 1");
        }
    }
    [TestClass]
    public class GetStockPERankTest
    {
        static IWebHost _webHost = null;
        static T GetService<T>()
        {
            var scope = _webHost.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
        }

        [TestMethod]
        public void 輸入的日期是假日()
        {
            var ctx = GetService<IStockInfoService>();
            DateTime date = new DateTime(2021, 04, 10);
            Assert.ThrowsException<MyTwseException>(() =>
            {
                ctx.GetStockPERank(date, count: 10);
            }, $"輸入的日期不能為假日：{date.ToString("yyyy-MM-dd")}");
        }
    }
}
