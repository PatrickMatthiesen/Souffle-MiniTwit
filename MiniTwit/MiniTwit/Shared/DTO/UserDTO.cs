using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTwit.Shared.DTO;
public record UserDTO (int id, string Name, string Email);
public record UserCreateDTO (int id, [StringLength(50)] string Name, [EmailAddress, StringLength(50)] string Email);
public record UserUpdateDTO (int id, [StringLength(50)] string Name, [EmailAddress, StringLength(50)] string Email);
