using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Shared.DTO;


public record UserDTO(string Id, string Name, string Email);
public record UserCreateDTO(string Id, [StringLength(50)] string Name, [EmailAddress, StringLength(50)] string Email);
public record UserUpdateDTO(string Id, [StringLength(50)] string Name, [EmailAddress, StringLength(50)] string Email);
