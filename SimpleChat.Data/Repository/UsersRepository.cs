using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SimpleChat.Core.Models;
using SimpleChat.Data.Bases;

namespace SimpleChat.Data.Repository
{
    public class UsersRepository : RepositoryBase<User>
    {
        public async Task<User> LoginAsync(string login, string password)
        {
            var user = await DbSet.FirstOrDefaultAsync(u => u.Login == login);

            if (user != null && user.Password == HashPassword(password))
                return user;

            return null;
        }

        public async Task<bool> CheckLoginExists(string login)
        {
            return await DbSet.AnyAsync(u => string.Equals(u.Login, login, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public static string HashPassword(string password)
        {
            var md5 = MD5.Create();

            return string.Join("", md5.ComputeHash(new MemoryStream(Encoding.Unicode.GetBytes(password))).Select(x => x.ToString("x2")));
        }
    }
}