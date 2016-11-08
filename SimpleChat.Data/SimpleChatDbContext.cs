using System.Data.Entity;
using SimpleChat.Data.Configuration;

namespace SimpleChat.Data
{
    public class SimpleChatDbContext : DbContext
    {
        public SimpleChatDbContext() : base("Default")
        {
            Database.SetInitializer<SimpleChatDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ChatMessageConfiguration());
        }
    }
}