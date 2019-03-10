using System;
using System.Security.Claims;
using WebShop.Models;

namespace WebShop.Extensions.IdentityExtensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            return user.Identity.Name;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            string role = "";

            if (user.IsInRole(Enum.GetName(typeof(Roles), Roles.Admin)))
            {
                role = "Admin";
            }
            else if (user.IsInRole(Enum.GetName(typeof(Roles), Roles.Client)))
            {
                role = "Client";
            }
            else
            {
                role = "Guest";
            }
            return role;
        }
    }
}
