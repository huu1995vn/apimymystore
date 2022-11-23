namespace APIMyMyStore.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIMyMyStore.Models;
using APIMyMyStore.Entites;

public interface ITokenService
{
    TokenResponse CreateToken(TokenRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
}

public class TokenService : ITokenService
{
    // admins hardcoded for simplicity, store in a db with hashed passwords in production applications
    DBLibrary.TemplateDAL _dal = null;

    public TokenService()
    {
        _dal = new DBLibrary.TemplateDAL(Variables.ConnectionSQL, "Users");

    }

    public TokenResponse CreateToken(TokenRequest model)
    {
        String password = CommonMethods.GetEncryptMD5(model.password);
        var dataset = _dal.GetAllByQuery($"Select * from public.\"Users\" where  (\"Phone\" = '{model.username}' OR \"Email\" = '{model.username}') AND \"Password\" = '{password}'");
        var users = CommonMethods.ConvertToEntity<User>(dataset);
        // return null if admin not found
        if (users.Count == 0) return null;
        User user = users[0];
        // authentication successful so generate jwt token
        var token = GenerateJwtToken(user);

        return new TokenResponse(user, token);
    }

    public IEnumerable<User> GetAll()
    {
        var dataset = _dal.GetAll();
        var users = CommonMethods.ConvertToEntity<User>(dataset);
        return users;
    }

    public User GetById(int id)
    {
        var dataset = _dal.GetAllById(id);
        var users = CommonMethods.ConvertToEntity<User>(dataset);
        if(users.Count == 0) return null;
        return users[0];
    }

    // helper methods

    private string GenerateJwtToken(User admin)
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