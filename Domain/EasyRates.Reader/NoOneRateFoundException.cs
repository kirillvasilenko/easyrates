using System;

namespace EasyRates.Reader.Model
{
    public class NoOneRateFoundException : Exception
    {
        public string From { get; }

        public NoOneRateFoundException(string from) : base($"No one rate found from {from}")
        {
            From = from;
        }
    }
}