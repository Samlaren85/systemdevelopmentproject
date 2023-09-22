using EntityLayer;

namespace DataLayer
{
    public class UnitOfWork
    {
        protected AppDbContext appDbContext { get; }
        public Repository<User> UserRepository { get; private set; } //nu finns du med

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
