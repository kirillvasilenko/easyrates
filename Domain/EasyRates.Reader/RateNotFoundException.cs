using System;

namespace EasyRates.Reader.Model
{
    public class RateNotFoundException : Exception
    {
        public string From { get; }
        public string To { get; }

        public RateNotFoundException(string from, string to):base($"Rate from {from} to {to} not found.")
        {
            From = @from;
            To = to;
        }
    }
}