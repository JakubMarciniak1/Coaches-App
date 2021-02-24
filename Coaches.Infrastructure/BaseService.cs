using System;
using System.Collections.Generic;
using System.Text;

namespace Coaches.Infrastructure
{
    public abstract class BaseService
    {
        const string Error500_ExceptionThrownWhileExecuting = "Exception thrown while executing the request. Details:\n";

        protected ServiceResponse TryExecute(Func<ServiceResponse> action)
        {
            try
            {
                return action();
            }
            catch(Exception ex)
            {
                return ServiceResponse.Error(new ErrorDetails(500, $"{Error500_ExceptionThrownWhileExecuting}{ex}"));
            }
        }

        protected ServiceResponse<T> TryExecute<T> (Func<ServiceResponse<T>> action) 
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                return ServiceResponse<T>.Error(new ErrorDetails(500, $"{Error500_ExceptionThrownWhileExecuting}{ex}"));
            }
        }

    }
}
