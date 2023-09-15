using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UnitOfWork
    {
        protected AppDbContext appDbContext { get; }

        public UnitOfWork()
        {
            appDbContext = new AppDbContext();

        }

        public void Delete()
        {
            //Not implemented!
        }
        public void Save()
        {
            appDbContext.SaveChanges();
        }
    }
}
