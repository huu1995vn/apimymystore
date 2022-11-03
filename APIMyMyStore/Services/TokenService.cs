namespace APIMyMyStore.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIMyMyStore.Helpers;
using APIMyMyStore.Models;
using APIMyMyStore.Entites;
using APIMyMyStore.DataAccess;

public interface ITokenService
{
    TokenResponse CreateToken(TokenRequest model);
    IEnumerable<Admin> GetAll();
    Admin GetById(int id);
}

public class TokenService : ITokenService
{
    // admins hardcoded for simplicity, store in a db with hashed passwords in production applications

    private readonly PostgreSqlContext _context;

    public TokenService(PostgreSqlContext context)
    {
        _context = context;
    }

    public TokenResponse CreateToken(TokenRequest model)
    {
        String password = CommonMethods.GetEncryptMD5(model.password);
        var admin = _context.Admins.SingleOrDefault(x => (x.phone == model.username || x.email == model.username) && x.password == password);

        // return null if admin not found
        if (admin == null) return null;

        // authentication successful so generate jwt token
        var token = GenerateJwtToken(admin);

        return new TokenResponse(admin, token);
    }

    public IEnumerable<Admin> GetAll()
    {
        return _context.Admins;
    }

    public Admin GetById(int id)
    {
        return _context.Admins.FirstOrDefault(x => x.id == id);
    }

    // helper methods

    private string GenerateJwtToken(Admin admin)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(CommonConstants.TOKEN_SECURITY_KEY);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", admin.id.ToString())}),
            Expires = DateTime.UtcNow.AddDays(CommonConstants.TOKEN_DURATION),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}