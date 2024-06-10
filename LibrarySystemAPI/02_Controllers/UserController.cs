using Microsoft.AspNetCore.Mvc;
using LibrarySystem.API.Models;
using LibrarySystem.API.Services;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
 private readonly IUserService _userService;

 public UserController(IUserService userServiceFromBuilder) 
    {
        _userService = userServiceFromBuilder;
    }

 [HttpPost("Users/{username}")]
  public async Task<ActionResult<User>> PostNewUser(string username)
    {
             User newUser = new User(username); //From Controller

             await _userService.CreateNewUserAsync(newUser);

             return Ok(newUser);

    }

 [HttpDelete("Users/{username}")]
  public async Task<ActionResult<string>> DeleteUser(string username)
    {

             await _userService.DeleteUserAsync(username);

             return Ok($"User {username} got deleted");

    }
}