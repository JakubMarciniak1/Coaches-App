﻿using System;

namespace Coaches.Infrastructure
{
    public class ErrorDetails
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ErrorDetails()
        {
            
        }

        public ErrorDetails(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
