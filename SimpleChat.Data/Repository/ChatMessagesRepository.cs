using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SimpleChat.Core.Models;
using SimpleChat.Data.Bases;

namespace SimpleChat.Data.Repository
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessage>
    {
        public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(int? top = null)
        {
            var query = top.HasValue ? DbSet.OrderByDescending(m => m.Sent).Take(top.Value) : DbSet.OrderBy(m => m.Sent);

            var messages = await query.ToListAsync();

            if (top.HasValue)
                messages = messages.OrderBy(m => m.Sent).ToList();

            return messages;
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            DbSet.Add(message);
            await DbContext.SaveChangesAsync();
        }
    }
}