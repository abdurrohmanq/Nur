using Nur.Domain.Enums;

namespace Nur.Application.UseCases.Users.DTOs;

public class UserDTO
{
    public long Id { get; set; }
    public long? TelegramId { get; set; }
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string LanguageCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}
