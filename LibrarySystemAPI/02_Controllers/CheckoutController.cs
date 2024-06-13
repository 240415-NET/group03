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
    public async Task<ActionResult> booksAvailableForCheckOut()
    {
        await _checkoutService.booksAvailableForCheckoutAsync();
        return Ok("List of available books");
    }
}