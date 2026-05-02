using PharmoSys.Core.Models;

namespace PharmoSys.Core.Store
{
    public static class AppSession
    {
        public static User CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser?.Role == "Admin";
        public static bool IsCashier => CurrentUser?.Role == "Cashier";
        public static bool IsManager => CurrentUser?.Role == "Manager";

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
