
namespace APIMyMyStore.Models;
using APIMyMyStore.Entites;

public class TokenResponse
{
    public int id { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string name { get; set; }
    public string token { get; set; }


    public TokenResponse(User admin, string ptoken)
    {
        id = admin.id;
        email = admin.email;
        phone = admin.phone;
        name = admin.name;
        token = ptoken;
    }
}