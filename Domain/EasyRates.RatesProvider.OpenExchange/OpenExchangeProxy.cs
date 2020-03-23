using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeProxy:IOpenExchangeProxy
    {
        private readonly string appId;
        private const string BaseUrl = "https://openexchangerates.org/api";

        private const string Latest = "latest.json";

        private const string AppIdParam = "app_id";

        private const string BaseParam = "base";
        
        
        public OpenExchangeProxy(string appId)
        {
            this.appId = appId;
        }
        
        public async Task<LatestRateResponse> GetCurrentRates(string from)
        {
            var url = $"{BaseUrl}/{Latest}";
            
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter(AppIdParam, appId);
            request.AddQueryParameter(BaseParam, from);

            var response = await client.ExecuteGetTaskAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<LatestRateResponse>(response.Content);
            }
            
            var err = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            throw new ErrorOnResponseToOpenExchangeException(err.Status, err.Message,err.Description, response.ResponseUri.ToString());
        }
    }
}