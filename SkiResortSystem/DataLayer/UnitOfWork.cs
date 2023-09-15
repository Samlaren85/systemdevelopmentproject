using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitylayer;

namespace DataLayer
{
    public class UnitOfWork
    {
        protected AppDbContext appDbContext { get; }
        public Repository<User> UserRepository { get; private set; }

        public UnitOfWork()
        {
            appDbContext = new AppDbContext();

            UserRepository = new Repository<User>(appDbContext);

        }
        public void Save()
        {
            appDbContext.SaveChanges();
        }
    }
}
