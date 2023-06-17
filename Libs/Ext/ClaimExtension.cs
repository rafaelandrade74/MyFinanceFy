using System.Security.Claims;

namespace MyFinanceFy.Libs.Ext
{
    public static class ClaimExtension
    {
        public static string Id(this ClaimsPrincipal User)
        {
            return User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
