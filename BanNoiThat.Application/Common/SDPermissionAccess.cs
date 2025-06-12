namespace BanNoiThat.Application.Common
{
    public static class SDPermissionAccess
    {
        public static string Manage = "manage";
        //Permission
        public static string ManageBrand = "manage-brand";
        public static string ManageCategory = "manage-category";
        public static string ManageProduct = "manage-product";
        public static string ManageUser = "manage-user";
        public static string ManageOrder = "manage-order";
        public static string ManageSaleProgram = "manage-saleprogram";
        public static string ManageAnalyst = "manage-analyst";
        public static string ManageRole = "manage-role";

        public static string BlockUser = "block-user";
        public const string CancelOrder = "cancel-order";


        public static Dictionary<string, string> Permissions = new()
    {
        {nameof(BlockUser), BlockUser},
        {nameof(ManageUser), ManageUser},
        {nameof(ManageProduct), ManageProduct},
        {nameof(ManageOrder), ManageOrder},
        {nameof(CancelOrder), CancelOrder},
        {nameof(ManageBrand), ManageBrand},
        {nameof(ManageCategory), ManageCategory},
        {nameof(ManageSaleProgram), ManageSaleProgram},
        {nameof(ManageAnalyst), ManageAnalyst},
        {nameof(ManageRole), ManageRole},
        {nameof(Manage), Manage}
    };
    }
}
