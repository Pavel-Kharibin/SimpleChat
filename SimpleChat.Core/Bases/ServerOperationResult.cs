using SimpleChat.Core.Enums;

namespace SimpleChat.Core.Bases
{
    public abstract class ServerOperationResult
    {
        protected ServerOperationResult()
        {
            OperationResult = OperationResult.Success;
            Message = "Ok";
        }

        public OperationResult OperationResult { get; set; }
        public string Message { get; set; }
    }
}