using System;
using System.Collections.Generic;
using YandexTank.PhantomAmmo;

namespace EasyRates.Reader.LoadTests
{
    public class RequestsMaker
    {
        private readonly string[] fromRates;
        private readonly string[] toRates;

        public RequestsMaker(string[] fromRates, string[] toRates)
        {
            this.fromRates = fromRates;
            this.toRates = toRates;
        }
        
        private readonly Random r = new Random(); 
        public int GetCount { get; private set; }
        
        public Func<PhantomAmmoInfo>[] MakeGets()
        {
            return new Func<PhantomAmmoInfo>[]
            {
                () =>
                {
                    GetCount++;

                    var from = fromRates[r.Next(0, fromRates.Length)];
                    var to = toRates[r.Next(0, toRates.Length)];
                    
                    var url = $"/api/v2/rates/{from}";

                    if (r.NextDouble() < 0.95)
                    {
                        url += $"/{to}";
                    }
                    
                    return PhantomAmmoInfo.MakeGet(url, new Dictionary<string, string>());
                }
            };
        }
    }
}