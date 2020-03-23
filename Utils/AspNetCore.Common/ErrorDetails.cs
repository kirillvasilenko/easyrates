namespace AspNetCore.Common
{
    /// <summary>
    /// Error details
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Makes <see cref="ErrorDetails"/>
        /// </summary>
        /// <param name="status"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ErrorDetails Make(
            int status,
            string code,
            string message)
        {
            return new ErrorDetails
            {
                Status = status,
                Code = code,
                Message = message
            };
        } 
        
        /// <summary>
        /// Http status code
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// Internal error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Human readable message
        /// </summary>
        public string Message { get; set; }
    }
}