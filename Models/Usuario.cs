using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyFinanceFy.Models
{
    public class Usuario : IdentityUser
    {
        [MaxLength(128)]
        public string? FullName { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    }
}
