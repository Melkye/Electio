using Microsoft.AspNetCore.Identity;

namespace Electio.DataAccess.Identity;
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}