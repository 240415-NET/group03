using Microsoft.AspNetCore.Mvc;
using LibrarySystem.API.Models;
using LibrarySystem.API.Services;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
 private readonly IUserService _userService;

 public UserController(IUserService userServiceFromBuilder) 
    {
        _userService = userServiceFromBuilder;
    }

 [HttpPost("{username}")]
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

   [HttpPatch("UserPatch")]
    public async Task<ActionResult> UpdateUserByUsername(UsernameUpdateDTO usernamesToSwap)
    {
          await _userService.UpdateUsernameAsync(usernamesToSwap);

          return Ok($"{usernamesToSwap.oldUserName}'s username has been changed to: {usernamesToSwap.newUserName}");
    } 

     [HttpGet("/Users/{username}")] 
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {   
        //Again, we are going to start with a try catch, so that we can NOT crash our API if something goes wrong,
        //and ideally, we can inform the front end so it can inform the user
        try
        {
            return await _userService.GetUserByUsernameAsync(username);
        }
        catch(Exception e)
        {
            return NotFound(e.Message);
        }
    }
}