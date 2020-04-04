using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.AuthorizationRequriement
{
    public class FakeXiechengRequireClaim : IAuthorizationRequirement
    {
        public FakeXiechengRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }

        public class FakeXiechengRequireClaimHandler : AuthorizationHandler<FakeXiechengRequireClaim>
        {
            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                FakeXiechengRequireClaim requirement
            )
            {
                var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
                if (hasClaim)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;

            }
        }
    }
}
