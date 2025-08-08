using System.ComponentModel.DataAnnotations;

namespace BMS_API.Dto;

public class LoginDto
{
    [Required(ErrorMessage="Username required")]
    
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password required")]
    public string Password { get; set; } = string.Empty;
}
