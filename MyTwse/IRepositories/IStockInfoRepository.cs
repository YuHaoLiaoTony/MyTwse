using MyTwse.Models;
using MyTwse.Repositories;
using System.Collections.Generic;

namespace MyTwse.IRepositories
{
    public interface IStockInfoRepository : IBaseRepository<StockInfo>
    {
        void Create(List<StockInfo> stockInfos, List<InsertDateLog> insertDateLogs);
    }
}
