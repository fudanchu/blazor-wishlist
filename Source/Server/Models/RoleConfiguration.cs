using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wishlist.Shared.Models.Security;

namespace Wishlist.Server.Models
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = IdentityRoles.Basic,
                    NormalizedName = IdentityRoles.Basic.ToUpper(),
                    Id = IdentityRoles.BasicId,
                    ConcurrencyStamp = IdentityRoles.BasicId
                },
                new IdentityRole
                {
                    Name = IdentityRoles.Admin,
                    NormalizedName = IdentityRoles.Admin.ToUpper(),
                    Id = IdentityRoles.AdminId,
                    ConcurrencyStamp = IdentityRoles.AdminId
                }
            );
        }
    }
}
