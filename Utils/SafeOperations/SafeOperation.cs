using System;

namespace SafeOperations
{
    public class SafeOperation
    {
        public static SafeOperationResult Run(Action action, SafeOperationMode mode = SafeOperationMode.ErrorIfException)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                return mode == SafeOperationMode.ErrorIfException
                    ? Error(e) 
                    : Warning(e);
            }

            return Success();
        }
        
        public static SafeOperationResult Error(Exception error)
        {
            var operationResult = new SafeOperationResult();
            operationResult.Errors.Add(error);
            return operationResult;
        }
        
        public static SafeOperationResult Warning(Exception warning)
        {
            var operationResult = new SafeOperationResult();
            operationResult.Warnings.Add(warning);
            return operationResult;
        }
        
        public static SafeOperationResult<T> Error<T>(T result, Exception error)
        {
            var operationResult = new SafeOperationResult<T>
            {
                Value = result
            };
            operationResult.Errors.Add(error);
            return operationResult;
        } 
        
        public static SafeOperationResult<T> Warning<T>(T result, Exception warning)
        {
            var operationResult = new SafeOperationResult<T>
            {
                Value = result
            };
            operationResult.Warnings.Add(warning);
            return operationResult;
        } 
        
        public static SafeOperationResult Success()
        {
            return new SafeOperationResult();
        }
        
        public static SafeOperationResult<T> Success<T>(T result)
        {
            return new SafeOperationResult<T>
            {
                Value = result
            };
        }

        public static SafeOperationResult FromMany(params SafeOperationResult[] results)
        {
            return Success().IncludeMany(results);
        }
    }
}