using System;

namespace Coaches.Infrastructure
{
    public class ErrorDetails
    {
        public int Code { get;}
        public string Message { get; }

        public ErrorDetails(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
