using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;

namespace Common
{
    public sealed class CanteenService : ICanteenService
    {
        public IAsyncOperation<IEnumerable<Canteen>> GetCanteens(string nameFilter, string dishFilter)
        {
            return this.GetCanteensHelper(nameFilter, dishFilter).AsAsyncOperation();
        }

        private async Task<IEnumerable<Canteen>> GetCanteensHelper(string nameFilter, string dishFilter)
        {
            using (var httpClient = new HttpClient())
            {
                // consider parameters
                var filterParams = new List<string>();

                if (!string.IsNullOrWhiteSpace(nameFilter))
                {
                    filterParams.Add($"nameFilter={nameFilter}");
                }

                if (!string.IsNullOrWhiteSpace(dishFilter))
                {
                    filterParams.Add($"dishFilter={dishFilter}");
                }

                // build correct filter URL
                var additionalFilters = string.Empty;

                if (filterParams.Any())
                {
                    additionalFilters = $"?{string.Join("&", filterParams)}";
                }

                var response = await httpClient.GetStringAsync(new Uri($"http://canteenchecker.azurewebsites.net/Public/Canteen{additionalFilters}"));

                var canteens = JsonConvert.DeserializeObject<IEnumerable<Canteen>>(response);

               return canteens;
            }
        }
    }
}
