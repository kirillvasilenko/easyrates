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
                rate.CurrencyFrom = formatter.FormatName(rate.CurrencyFrom);
                rate.CurrencyTo = formatter.FormatName(rate.CurrencyTo);
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