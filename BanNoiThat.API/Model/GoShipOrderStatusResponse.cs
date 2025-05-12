namespace BanNoiThat.API.Model
{
    public class GoShipOrderStatusResponse
    {
        public string Gcode { get; set; }
        public string Code { get; set; }
        public string OrderId { get; set; }
        public double Weight { get; set; }
        public int Fee { get; set; }
        public int Cod { get; set; }
        public int Payer { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string TrackingUrl { get; set; }

        public override string ToString()
        {
            return $"Order ID: {OrderId}\n" +
                   $"Tracking Code: {Code} ({Gcode})\n" +
                   $"Weight: {Weight}g\n" +
                   $"Fee: {Fee} VND\n" +
                   $"COD: {Cod} VND\n" +
                   $"Payer: {Payer}\n" +
                   $"Status: {Status} - {Message}\n" +
                   $"Tracking URL: {TrackingUrl}";
        }
    }
}
