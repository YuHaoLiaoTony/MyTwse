using MyTwse.IRepositories;
using MyTwse.Models;
using System.Collections.Generic;

namespace MyTwse.Repositories
{
    public class StockInfoRepository : BaseTwseStockRepository<StockInfo>, IStockInfoRepository
    {
        public void Create(List<StockInfo> stockInfos, List<InsertDateLog> insertDateLogs)
        {
            foreach (var item in stockInfos)
            {
                _DB.Add(item);
            }
            foreach (var item in insertDateLogs)
            {
                _DB.Add(item);
            }
            _DB.SaveChanges();
        }
    }
}
