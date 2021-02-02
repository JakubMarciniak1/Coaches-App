using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Coaches.Infrastructure
{
    public class ServiceResponse
    {
        public ErrorDetails ErrorDetails { get; }

        public bool IsSuccess => ErrorDetails == null;

        protected ServiceResponse(ErrorDetails errorDetails)
        {
            ErrorDetails = errorDetails;
        }

        public static ServiceResponse Success()
        {
            return new ServiceResponse(null);
        }

        public static ServiceResponse Error(ErrorDetails errorDetails)
        {
            return new ServiceResponse(errorDetails);
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T ResponseDTO { get; } //DTO = Data Transfer Object

        private ServiceResponse(T responseDTO)
        : base(null)
        {
            ResponseDTO = responseDTO;
        }

        public static ServiceResponse<T> Success(T responseDto)
        {
            return new ServiceResponse<T>(responseDto);
        }
    }
}
