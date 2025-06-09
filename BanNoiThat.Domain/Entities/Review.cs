namespace BanNoiThat.Domain.Entities
{
    public class Review
    {
        public string Id { get; set; }
        public string User_Id { get; set; }
        public User User { get; set; }
        public string Product_Id { get; set; }
        public Product Product { get; set; }
        public string Comment { get; set; } 
        public double Rate { get; set; }
        public string IsShow { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
