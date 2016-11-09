using System.Collections.Generic;
using SimpleChat.Core.Bases;
using SimpleChat.Core.Models;

namespace SimpleChat.Core.OperationResults
{
    public class LoadMessagesOperationResult : ServerOperationResult
    {
         public IEnumerable<ChatMessage> Messages { get; set; }
    }
}