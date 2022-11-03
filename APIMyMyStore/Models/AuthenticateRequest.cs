namespace APIMyMyStore.Models;

using System.ComponentModel.DataAnnotations;

public class TokenRequest
{
    [Required]
    public string username { get; set; }

    [Required]
    public string password { get; set; }
}