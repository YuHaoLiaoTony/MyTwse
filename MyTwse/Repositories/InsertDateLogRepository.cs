using MyTwse.IRepositories;
using MyTwse.Models;

namespace MyTwse.Repositories
{
    public class InsertDateLogRepository : BaseTwseStockRepository<InsertDateLog>, IInsertDateLogRepository
    {
        public InsertDateLogRepository(TwseStockContext context) : base(context)
        {

        }
    }
}
