using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Reader.Model;
using EasyRates.ReaderApp.Dto;

namespace EasyRates.ReaderApp
{
    public class ReaderRateService : IReaderRateService
    {
        private readonly IRatesReader reader;
        private readonly ICurrencyNameFormatter formatter;
        private readonly ICurrencyNameValidator validator;

        public ReaderRateService(
            IRatesReader reader,
            ICurrencyNameFormatter formatter,
            ICurrencyNameValidator validator)
        {
            this.reader = reader;
            this.formatter = formatter;
            this.validator = validator;
        }
        
        public async Task<RatesResponse> GetRate(string @from, string to)
        {
            from = formatter.FormatName(from);
            to = formatter.FormatName(to);
            
            validator.Validate(from);
            validator.Validate(to);
            
            var result = await reader.GetRate(from, to);
            return result.ToDto();
        }
        
        public async Task<RatesResponse> GetRates(string @from)
        {
            from = formatter.FormatName(from);
            validator.Validate(from);
            
            var result = await reader.GetRates(from);
            return result.ToDto();
        }
    }
}