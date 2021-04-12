using MyTwse.IRepositories;
using MyTwse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTwse.Repositories
{
    public class BaseTwseStockRepository<T> : BaseRepository<T> where T : class
    {
        protected new TwseStockContext _DB = null;
        public BaseTwseStockRepository(TwseStockContext context) : base(context)
        {
            _DB = (TwseStockContext)base._DB;
        }
    }
}
