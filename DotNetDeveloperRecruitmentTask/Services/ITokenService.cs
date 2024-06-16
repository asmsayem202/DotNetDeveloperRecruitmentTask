using DotNetDeveloperRecruitmentTask.Models;
using Microsoft.AspNetCore.Identity;

namespace DotNetDeveloperRecruitmentTask.Services
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}