using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeProxy:IOpenExchangeProxy
    {
        private const string BaseUrl = "https://openexchangerates.org/api";

        private const string Latest = "latest.json";

        private const string Usage = "usage.json";

        private const string AppIdParam = "app_id";

        private const string BaseParam = "base";
        
        private readonly string appId;
        
        public OpenExchangeProxy(string appId)
        {
            this.appId = appId;
        }
        
        public async Task<ActualRateResponse> GetCurrentRates(string from)
        {
            var url = $"{BaseUrl}/{Latest}";
            
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter(AppIdParam, appId);
            request.AddQueryParameter(BaseParam, from);

            var response = await client.ExecuteGetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActualRateResponse>(response.Content);
            }
            
            var err = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            throw new ErrorOnResponseToOpenExchangeException(err.Status, err.Message,err.Description, response.ResponseUri.ToString());
        }
        
        public async Task<string> GetUsage()
        {
            var url = $"{BaseUrl}/{Usage}";
            
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter(AppIdParam, appId);

            var response = await client.ExecuteGetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            
            var err = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            throw new ErrorOnResponseToOpenExchangeException(err.Status, err.Message,err.Description, response.ResponseUri.ToString());
        }
    }
}