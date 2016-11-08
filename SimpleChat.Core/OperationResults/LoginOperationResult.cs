using SimpleChat.Core.Bases;
using SimpleChat.Core.Models;

namespace SimpleChat.Core.OperationResults
{
    public class LoginOperationResult : ServerOperationResult
    {
        public User User { get; set; }
    }
}