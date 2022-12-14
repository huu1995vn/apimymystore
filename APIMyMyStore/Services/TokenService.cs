using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIMyMyStore.Entites;
using Newtonsoft.Json;

namespace APIMyMyStore.Services;
public interface ITokenService
{
    String CreateToken(string pusername, string ppasswordmp5);
    String RefreshToken(string token);
    int RemoveToken(string token);
    IEnumerable<User> GetAll();
    User GetById(int id);
    User GetTokenInfo(string token);

}

public class TokenService : ITokenService
{
    // admins hardcoded for simplicity, store in a db with hashed passwords in production applications
    DBLibrary.TemplateDAL _dal = null;

    public TokenService()
    {
        _dal = new DBLibrary.TemplateDAL(Variables.ConnectionSQL, "users");

    }

    public string CreateToken(string username, string password)
    {
        try
        {
            var dataset = _dal.GetAllByQuery($"Select * from public.\"users\" where  (\"phone\" = '{username}' OR \"email\" = '{username}') AND \"password\" = '{password}'");
            var users = CommonMethods.ConvertToEntity<User>(dataset);
            // return null if admin not found
            if (users.Count == 0) throw new Exception(CommonConstants.MESSAGE_LOGIN_FAIL);

            User user = users[0];
            if (user.status != 1) throw new Exception(CommonConstants.MESSAGE_USER_NOT_VALID);

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            _dal.Update(user.id, new string[] { "Token" }, new object[] { token });
            return token;
        }
        catch (System.Exception ex)
        {
            CommonMethods.WriteLog(ex.Message);
            throw new Exception(CommonConstants.MESSAGE_USER_NOT_VALID);
        }


    }

    public String RefreshToken(string pToken)
    {
        try
        {
            var dataset = _dal.GetAllByQuery($"Select * from public.\"users\" where \"token\" = '{pToken}'");
            var users = CommonMethods.ConvertToEntity<User>(dataset);
            // return null if admin not found
            if (users.Count == 0) throw new Exception(CommonConstants.MESSAGE_LOGIN_FAIL);
            User user = users[0];
            if (user.status != 1) throw new Exception(CommonConstants.MESSAGE_USER_NOT_VALID);
            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            _dal.Update(user.id, new string[] { "Token" }, new object[] { token });
            return token;
        }
        catch (System.Exception ex)
        {
            CommonMethods.WriteLog(ex.Message);
            throw new Exception(CommonConstants.MESSAGE_USER_NOT_VALID);
        }


    }

    public int RemoveToken(string pToken)
    {
        try
        {
            var dataset = _dal.GetAllByQuery($"Select * from public.\"users\" where \"token\" = '{pToken}'");
            var users = CommonMethods.ConvertToEntity<User>(dataset);
            // return null if admin not found
            if (users.Count == 0) return -1;
            User user = users[0];
            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            return _dal.Update(user.id, new string[] { "Token" }, new object[] { string.Empty });
        }
        catch (System.Exception ex)
        {
            CommonMethods.WriteLog(ex.Message);
            throw new Exception(CommonConstants.MESSAGE_USER_NOT_VALID);
        }


    }

    public IEnumerable<User> GetAll()
    {
        var dataset = _dal.GetAll();
        var users = CommonMethods.ConvertToEntity<User>(dataset);
        return users;
    }

    public User GetById(int id)
    {
        var dataset = _dal.GetDataById(id, Variables.FieldSelectUser);
        var users = CommonMethods.ConvertToEntity<User>(dataset);
        if (users.Count == 0) return null;
        return users[0];
    }

    // helper methods

    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(CommonConstants.TOKEN_SECURITY_KEY);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
            new Claim("id", user.id.ToString()),
            new Claim("name", user.name!.ToString()),
            new Claim("phone", user.phone!.ToString()),
            new Claim("email", user.email!.ToString()),
            new Claim("status", user.status!.ToString()),
            new Claim("fileid", user.fileid!.ToString()),
            new Claim("address", user.address!.ToString()),
            new Claim("createdate", user.createdate.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(CommonConstants.TOKEN_DURATION),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public User GetTokenInfo(string token)
    {
        try
        {
            var TokenInfo = new Dictionary<string, string>();
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {

                TokenInfo.Add(claim.Type, claim.Value);
            }

            return JsonConvert.DeserializeObject<User>(CommonMethods.ConvertToJsonString(TokenInfo));
        }
        catch (System.Exception)
        {

            return null;
        }

    }
}