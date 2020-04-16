using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model.Ef.Pg;
using Microsoft.EntityFrameworkCore;
using YandexTank.PhantomAmmo;

namespace EasyRates.Reader.LoadTests
{
    class Program
    {
        private const string ConnectionString =
            "server=localhost;port=5433;database=easyrates;user id=easyrates_app;password=easyrates_app";

        private const string FileName = "ammo.txt";
        
        private const int RequestsCount = 10_000;
        
        static async Task Main(string[] args)
        {
            var context = new RatesContextFactory().CreateDbContext(ConnectionString);

            var fromRates = await context.ActualRates
                .Select(x => x.CurrencyFrom)
                .Distinct()
                .ToArrayAsync();
            
            var toRates = await context.ActualRates
                .Select(x => x.CurrencyTo)
                .Distinct()
                .ToArrayAsync();
            
            var requestMaker = new RequestsMaker(fromRates, toRates);

            var generator = new PhantomAmmoGeneratorBuilder()
                .AddSources(requestMaker.MakeGets())
                .Build();
            
            using (var file = File.CreateText(GetFilePath()))
            {
                for (int i = 0; i < RequestsCount; i++)
                {
                    file.Write(generator.GetNext());
                }    
            }
            
            Console.WriteLine($"Get:{requestMaker.GetCount}");

        }

        private static string GetFilePath()
        {
            var fullPath = Path.Combine("../../..", "tank", FileName);
            return Path.GetFullPath(fullPath);
        }
    }
}