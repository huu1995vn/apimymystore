
namespace APIMyMyStore.Models;
using APIMyMyStore.Entites;

public class TokenResponse
{
    public int id { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string name { get; set; }
    public string token { get; set; }


    public TokenResponse(User puser, string ptoken)
    {
        id = puser.id;
        email = puser.email;
        phone = puser.phone;
        name = puser.name;
        token = ptoken;
    }
}