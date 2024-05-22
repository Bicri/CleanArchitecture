using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.HasData(
            new ApplicationUser
            {
                Id = "1a4f2e5d-2c81-437f-99c0-c9003c4df94f",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                Nombre = "Isaac Bicri",
                Apellidos = "Martinez",
                UserName = "Bicri",
                NormalizedUserName = "BICRI",
                PasswordHash = hasher.HashPassword(null, "Admin123*"),
                EmailConfirmed = true,
            },
            new ApplicationUser
            {
                Id = "199c2243-1657-4efa-a9a2-4846f6aae75e",
                Email = "user@localhost.com",
                NormalizedEmail = "USER@LOCALHOST.COM",
                Nombre = "Juan",
                Apellidos = "Perez",
                UserName = "Juan Perez",
                NormalizedUserName = "JUAN PEREZ",
                PasswordHash = hasher.HashPassword(null, "Admin123*"),
                EmailConfirmed = true,
            }
        );
    }
}

