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
                    Name = Roles.Basic,
                    NormalizedName = Roles.Basic.ToUpper()
                },
                new IdentityRole
                {
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper()
                }
            );
        }
    }
}
