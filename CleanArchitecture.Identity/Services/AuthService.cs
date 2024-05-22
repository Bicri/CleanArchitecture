using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Identity.Services;

internal class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JWTSettings _jwtSettings;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> Login(AuthRequest authRequest)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(authRequest.Email)
                                ?? throw new Exception($"El usuario con email {authRequest.Email} no existe");

        var resultado = await _signInManager.PasswordSignInAsync(user.UserName!, authRequest.Password, false, false);

        if(!resultado.Succeeded)
        {
            throw new Exception("Credenciales inválidas");
        }
        
        var token = await GenerateToken(user);
        AuthResponse authResponse = new()
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };

        return authResponse;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest registrationRequest)
    {
        var existingUser = _userManager.FindByNameAsync(registrationRequest.Username);

        if(existingUser is not null)
        {
           throw new Exception($"El usuario con nombre {registrationRequest.Username} ya existe");
        }

        var existingEmail = _userManager.FindByEmailAsync(registrationRequest.Email);

        if(existingEmail is not null)
        {
            throw new Exception($"El usuario con email {registrationRequest.Email} ya existe");
        }

        ApplicationUser user = new()
        {
            Email = registrationRequest.Email,
            Nombre = registrationRequest.Nombre,
            Apellidos = registrationRequest.Apellidos,
            UserName = registrationRequest.Username,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registrationRequest.Password);

        if(!result.Succeeded)
        {
            throw new Exception($"{result.Errors}");
        }

        await _userManager.AddToRoleAsync(user, "Operator");

        var token = await GenerateToken(user);

        return new RegistrationResponse
        {
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(CustomClaimTypes.Uid, user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: _signingCredentials
        );

        return jwtSecurityToken;
    }
}
