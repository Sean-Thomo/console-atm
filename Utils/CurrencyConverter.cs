using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetEnv;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Utils
{
    public class CurrencyConverter
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<decimal> Convert(string currencyCode, string targetCurrency)
        {
            Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("EXCHANGE_RATE_API_KEY");
            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{currencyCode}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            ExchangeRateResponse exchangeResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(responseBody);

            if (exchangeResponse.Result == "success")
            {
                Dictionary<string, decimal> conversionRates = exchangeResponse.ConversionRates;

                if (conversionRates.TryGetValue(targetCurrency, out decimal value))
                {
                    return (decimal)value;
                }
                else
                {
                    Console.WriteLine($"Currency {targetCurrency} not found in conversion rates.");
                }
                return 0.00M;
            }
            else
            {
                Console.WriteLine("API call was not successful.");
                return 0.00M;
            }
        }
    }
}
