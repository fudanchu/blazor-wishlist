using Microsoft.AspNetCore.Authorization;
using System;

namespace Wishlist.Shared.Models.Security
{
    public class Policies
    {
        public const string CanEditUsers = "CanEditUsers";
        public const string CanImpersonate = "CanImpersonate";
        public const string CanCreateAdmins = "CanCreateAdmins";
        public const string CanCreateUsers = "CanCreateUsers";
        public const string CanViewAllGifts = "CanViewAllGifts";
        public const string CanEditAllGifts = "CanEditAllGifts";

        public static Action<AuthorizationOptions> ConfigurePolicies()
        {
            return config =>
            {
                config.AddPolicy(CanEditUsers, IsAdminUserPolicy());
                config.AddPolicy(CanCreateUsers, IsAdminUserPolicy());
                config.AddPolicy(CanImpersonate, IsAdminUserPolicy());
                config.AddPolicy(CanCreateAdmins, IsAdminUserPolicy());
                config.AddPolicy(CanViewAllGifts, IsAdminUserPolicy());
                config.AddPolicy(CanEditAllGifts, IsAdminUserPolicy());
            };
        }

        public static AuthorizationPolicy IsAdminUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Admin")
                                                   .Build();
        }

        public static AuthorizationPolicy IsBasicUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Basic")
                                                   .Build();
        }
    }
}