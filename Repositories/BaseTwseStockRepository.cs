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
        protected TwseStockContext _DB = null;
        public BaseTwseStockRepository() : base(new TwseStockContext())
        {
            _DB = (TwseStockContext)base._DB;
        }
    }
}
