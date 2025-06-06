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

        public async Task<CreateOrderResponse> CreateOrderGHN(string token, object payload)
        {
            try
            {
                // Endpoint URL
                var url = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create";

                // Convert payload to JSON
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                Console.WriteLine(jsonPayload);

                // Add headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", "196544");
                _httpClient.DefaultRequestHeaders.Host = "dev-online-gateway.ghn.vn";
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send POST request
                var response = await _httpClient.PostAsync(url, content);
                // Log thông tin lỗi nếu xảy ra
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
                    Console.WriteLine($"Response Body: {responseBody}");
                    throw new Exception("API returned an error. Check the response log for details.");
                }
                response.EnsureSuccessStatusCode();


                // Parse response content
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
                    Console.WriteLine($"Response Body: {responseBody}");
                    throw new Exception("API returned an error. Check the response log for details.");
                }
                response.EnsureSuccessStatusCode();
                var createOrderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(responseContent);

                return createOrderResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateOrderGHN: {ex.Message}");
                throw;
            }
        }

        public async Task<OrderDetailGHNReponse> GetStatusOrder(string token, string orderCode)
        {
            try
            {
                // Endpoint URL
                var url = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail";

                // Tạo payload JSON
                var payload = new
                {
                    order_code = orderCode
                };

                // Convert payload to JSON
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                Console.WriteLine(jsonPayload);

                // Add headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", token);
                _httpClient.DefaultRequestHeaders.Host = "dev-online-gateway.ghn.vn";
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var shippingFeeResponse = JsonConvert.DeserializeObject<OrderDetailGHNReponse>(responseContent);
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


    //Fee Shipping
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

    // Request Model
    public class CreateOrderRequest
    {
        public string ServiceId { get; set; }
        public string ToName { get; set; }
        public string ToPhone { get; set; }
        public string ToAddress { get; set; }
        public string ToWardCode { get; set; }
        public string ToDistrictId { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PaymentTypeId { get; set; }
        public int RequiredNote { get; set; }
        public string Content { get; set; }
        public int CodAmount { get; set; }
        // Thêm các trường cần thiết từ tài liệu
    }

    // Response Model
    public class CreateOrderResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public OrderShippingInfo Data { get; set; }
    }

    public class OrderShippingInfo
    {
        public string order_code { get; set; }
        public string total_fee { get; set; }
    }

    public class OrderDetailGHNReponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public OrderDetailGHN Data { get; set; }
    }

    public class OrderDetailGHN
    {
        public string shop_id { get; set; }
        public string status { get; set; }
    }
}
