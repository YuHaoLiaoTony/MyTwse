using System;
namespace MyTwseTest
{

    [TestClass]
    public class GetYieldRateIncreaseＭaxDays
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
        public void 開始日期大於結束日期()
        {
            var ctx = GetService<IStockInfoService>();
            Assert.ThrowsException<MyTwseException>(() =>
            {
                ctx.GetYieldRateIncreaseＭaxDays(new YieldRateIncreaseＭaxDaysQueryModel
                {
                    Code = "1439",
                    StartDate = new DateTime(2021, 04, 10),
                    EndDate = new DateTime(2021, 04, 09)
                });
            }, $"錯誤請求：開始時間不得大於結束時間");
        }
    }
}
