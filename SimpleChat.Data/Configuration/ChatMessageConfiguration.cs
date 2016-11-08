using System.Data.Entity.ModelConfiguration;
using SimpleChat.Core.Models;

namespace SimpleChat.Data.Configuration
{
    internal class ChatMessageConfiguration : EntityTypeConfiguration<ChatMessage>
    {
        public ChatMessageConfiguration()
        {
            ToTable("Messages");
            Property(p => p.Id).HasColumnName("ID");
            Property(p => p.UserId).HasColumnName("UserID");
            Property(p => p.Sent).HasColumnName("Sent");
        }
    }
}