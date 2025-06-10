namespace BanNoiThat.Application.Common
{
    public static class StaticDefine
    {
        public const string SD_Storage_Containter = "eventmanagement";

        //Claim user
        public const string Claim_User_Id = "user_id";
        public const string Claim_FullName = "fullName";
        public const string Claim_User_Role = "user_role";

        //Status order 
        //Pending, Processing, Shipping , Done, cancelled ,Returned (Đơn hàng hoàn lại)
        public const string Status_Order_Pending = "Pending";
        public const string Status_Order_Processing = "Processing";
        public const string Status_Order_Shipping = "Shipping";
        public const string Status_Order_Done = "Done";
        public const string Status_Order_Returned = "Returned";
        public const string Status_Order_Cancelled = "Cancelled";

        //Status payment
        //Pending, Paid, Failed, Refunded
        public const string Status_Payment_Pending = "Pending";
        public const string Status_Payment_Paid = "Paid";
        public const string Status_Payment_Failed = "Failed";
        public const string Status_Payment_Refunded = "Refunded";

        //Role
        public const string Role_Admin = "admin";
        public const string Role_Customer = "customer";
        public const string Role_Staff = "staff";

        //Coupon
        public const string DiscountType_Percent = "percent";
        public const string DiscountType_FixedAmount = "fixed_amount";

        //Coupon Type
        //onlyproduct, Together, onlyshipping
        public const string CouponType_OnlyCouponProduct = "onlyproduct";
        public const string CouponType_Together = "together";
        public const string CouponType_OnlyCouponShipping = "onlyshipping";

        //SaleType
        public const string Sale_ApplyType_Brand = "brand";
        public const string Sale_ApplyType_category = "category";

        public const string SD_URL_LINK_RESETPASSWORD = "http://localhost:3005/reset-password";
        public const string SD_URL_LINK_CONFIRMPASSWORD = "http://localhost:7000/api/auth/confirm-password";

        //Sale
        public const string SP_Status_Inactive = "inactive";
        public const string SP_Status_Active = "active";
        public const string SP_Status_Expired = "expired";

    }
}
