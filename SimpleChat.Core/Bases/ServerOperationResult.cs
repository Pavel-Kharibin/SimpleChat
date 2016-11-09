using System;
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

        public static T Failure<T>(string message) where T : ServerOperationResult
        {
            var result  = Activator.CreateInstance<T>();

            result.OperationResult = OperationResult.Failure;
            result.Message = message;

            return result;
        }

        public OperationResult OperationResult { get; set; }
        public string Message { get; set; }
    }
}