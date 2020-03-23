using System;
using System.Collections.Generic;
using EasyRates.Model;

namespace EasyRates.WriterApp
{
    public class CurrencyRateProcessor:ICurrencyRateProcessor
    {
        private readonly TimeSpan lagToExpiration = TimeSpan.FromSeconds(10);
        
        private readonly ICurrencyNameFormatter formatter;
        private readonly ITimetable timetable;

        public CurrencyRateProcessor(
            ICurrencyNameFormatter formatter,
            ITimetable timetable)
        {
            this.formatter = formatter;
            this.timetable = timetable;
        }
        
        public void Process(ICollection<CurrencyRate> rates)
        {
            FormatNames(rates);
            SetExpirationTime(rates);
        }
        
        private void FormatNames(ICollection<CurrencyRate> rates)
        {
            foreach (var rate in rates)
            {
                rate.From = formatter.FormatName(rate.From);
                rate.To = formatter.FormatName(rate.To);
            }
        }

        private void SetExpirationTime(ICollection<CurrencyRate> rates)
        {
            foreach (var rate in rates)
            {
                rate.ExpirationTime = timetable.GetNextMoment(rate.TimeOfReceipt) + lagToExpiration;
            }
        }
    }
}