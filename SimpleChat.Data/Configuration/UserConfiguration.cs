using System.Data.Entity.ModelConfiguration;
using SimpleChat.Core.Models;

namespace SimpleChat.Data.Configuration
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            Property(p => p.Id).HasColumnName("ID");
            Property(p => p.Name).HasColumnName("Name");
            Property(p => p.Login).HasColumnName("Login");
            Property(p => p.Password).HasColumnName("Password");
            Property(p => p.IsAdmin).HasColumnName("IsAdmin");
        }
    }
}