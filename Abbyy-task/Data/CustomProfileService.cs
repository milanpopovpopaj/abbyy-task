using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbyy_task.Data
{
    public class CustomProfileService : IProfileService
    {
        public CustomProfileService()
        { }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //get role claims from ClaimsPrincipal 
            //var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);

            //add your role claims 
            //context.IssuedClaims.AddRange(roleClaims);
            context.IssuedClaims.AddRange(context.Subject.Claims);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
