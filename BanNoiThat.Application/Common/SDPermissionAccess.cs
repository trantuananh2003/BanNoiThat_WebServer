namespace BanNoiThat.Application.Common
{
    public static class SDPermissionAccess
    {
        public static string Manage = "manage";
        //Permission
        public static string BlockUser = "block-user";
        public static string ManageUser = "manage-user";
        public static string ManageProduct = "manage-product";
        public static string ManageOrder = "manage-order";
        public const string CancelOrder = "cancel-order";

        public static Dictionary<string, string> Permissions = new()
        {
            {nameof(BlockUser), BlockUser},
            {nameof(ManageUser), ManageUser},
            {nameof(ManageProduct), ManageProduct },
            {nameof(ManageOrder), ManageOrder},
            {nameof(CancelOrder), CancelOrder}
        };
    }
}
