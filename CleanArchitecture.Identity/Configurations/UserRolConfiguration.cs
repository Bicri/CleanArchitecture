using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;

public class UserRolConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "ed162868-4a34-4401-b05c-a5b8140041e8",
                UserId = "1a4f2e5d-2c81-437f-99c0-c9003c4df94f"
            },
            new IdentityUserRole<string>
            {
                RoleId = "8b1a5aa0-c9c9-4019-a5af-dcb14d6daa97",
                UserId = "199c2243-1657-4efa-a9a2-4846f6aae75e"
            }
        );
    }
}
