namespace GRHelper.Web.Infrastructure
{
    using System.Security.Claims;

    using GRHelper.Common;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
           => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
           => user.IsInRole(GlobalConstants.AdministratorRoleName);

        public static string Email(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Email).Value;
    }
}
