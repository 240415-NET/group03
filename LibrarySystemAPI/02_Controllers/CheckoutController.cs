using Microsoft.AspNetCore.Mvc;
using LibrarySystem.API.Models;
using LibrarySystem.API.Services;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("Checkout")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutServiceFromBuilder)
    {
        _checkoutService = checkoutServiceFromBuilder;
    }

    [HttpPost]
    public async Task<ActionResult> PostNewCheckkout(checkoutDTO newCheckout)
    {
        await _checkoutService.CreateNewCheckoutAsync(newCheckout);
        return Ok("You are now checked out");
    }


    [HttpGet("Books")]
    public async Task<List<Book>> booksAvailableForCheckOut()
    {
        return await _checkoutService.booksAvailableForCheckoutAsync();
        
    }
    [HttpGet("/{userId}")]
    public async Task<ActionResult<List<Checkout>>> GetCheckedOutBooksbyUserId(Guid userId)
    {
        
         try
        {
            //Creating a list to eventually *hopefully* return to our front end
            List<Checkout> usersCheckedOutBooks = await _checkoutService.GetCheckedOutBooksbyUserIdAsync(userId);

            return Ok(usersCheckedOutBooks);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

  }
}