using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace WebShop.Views.Shared
{
    public static class UserProfileMenuNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Main => "Main";

        public static string ManageAccount => "Index";

        public static string Products => "Products";

        public static string CartDetails => "Details";

        public static string ManageUserProfile => "ManageUserProfile";

        public static string SendEmail => "SendEmail";

        public static string ManageAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageAccount);

        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Products);

        public static string CartDetailsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CartDetails);

        public static string UserProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageUserProfile);

        public static string MainNavClass(ViewContext viewContext) => PageNavClass(viewContext, Main);

        public static string SendEmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, SendEmail);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}