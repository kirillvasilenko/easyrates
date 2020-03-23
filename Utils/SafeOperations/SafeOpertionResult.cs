using System;
using System.Collections.Generic;
using System.Linq;

namespace SafeOperations
{
    public class SafeOperationResult
    {
        public bool IsSuccess => !HasErrors && !HasWarnings;
        
        public bool HasErrors => Errors.Any();
        
        public bool HasWarnings => Warnings.Any();

        public IList<Exception> Errors { get; } = new List<Exception>();

        public IList<Exception> Warnings { get; } = new List<Exception>();


        public void AddWarning(Exception warn)
        {
            Warnings.Add(warn);
        }

        public void AddError(Exception ex)
        {
            Errors.Add(ex);
        }
        
        public SafeOperationResult Include(SafeOperationResult result)
        {
            foreach (var error in result.Errors)
            {
                Errors.Add(error);
            }
            foreach (var error in result.Warnings)
            {
                Warnings.Add(error);
            }

            return this;
        }
        
        public SafeOperationResult IncludeMany(params SafeOperationResult[] results)
        {
            foreach (var result in results)
            {
                Include(result);
            }

            return this;
        }

    }

    public class SafeOperationResult<T> : SafeOperationResult
    {
        public T Value { get; set; }
        
        public new SafeOperationResult<T> Include(SafeOperationResult result)
        {
            base.Include(result);
            return this;
        }
        
        public new SafeOperationResult<T> IncludeMany(params SafeOperationResult[] results)
        {
            base.IncludeMany(results);
            return this;
        }
    }
}