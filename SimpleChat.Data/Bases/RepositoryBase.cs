using System.Data.Entity;

namespace SimpleChat.Data.Bases
{
    public abstract class RepositoryBase<T>  where T : class 
    {
        protected RepositoryBase()
        {
            DbSet = DbContext.Set<T>();
        }

        protected DbSet<T> DbSet { get; private set; }
        protected DbContext DbContext { get; } = new SimpleChatDbContext();
    }
}