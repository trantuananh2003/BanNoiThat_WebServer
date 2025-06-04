using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BanNoiThat.Application.Service.PaymentService
{
    public class ServiceShipping
    {
        private readonly HttpClient _httpClient;

        public ServiceShipping(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ShippingFeeResponse> CalculateShippingFeeAsync(string token, object payload)
        {
            try
            {
                // Endpoint URL
                var url = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";

                // Convert payload to JSON
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Add headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send POST request
                var response = await _httpClient.PostAsync(url, content);

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read response content
                var responseContent = await response.Content.ReadAsStringAsync();


                // Deserialize the JSON response into a C# object
                var shippingFeeResponse = JsonConvert.DeserializeObject<ShippingFeeResponse>(responseContent);

                // Return the deserialized object
                return shippingFeeResponse;
            }
            catch (Exception ex)
            {
                // Handle errors
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }

    public class ShippingFeeResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ShippingFeeData Data { get; set; }
    }

    public class ShippingFeeData
    {
        public int Total { get; set; }
        public int ServiceFee { get; set; }
        public int InsuranceFee { get; set; }
        public int PickStationFee { get; set; }
        public int CouponValue { get; set; }
        public int R2sFee { get; set; }
        public int ReturnAgain { get; set; }
        public int DocumentReturn { get; set; }
        public int DoubleCheck { get; set; }
        public int CodFee { get; set; }
        public int PickRemoteAreasFee { get; set; }
        public int DeliverRemoteAreasFee { get; set; }
        public int CodFailedFee { get; set; }
    }
}
