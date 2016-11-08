using System.Collections.Generic;
using SimpleChat.Core.Models;

namespace SimpleChat.Core.OperationResults
{
    public class LoadMessagesOperationResult
    {
         public IEnumerable<ChatMessage> Messages { get; set; }
    }
}