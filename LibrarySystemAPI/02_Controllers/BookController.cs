using Microsoft.AspNetCore.Mvc;
using LibrarySystem.API.Models;
using LibrarySystem.API.Services;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("Books")]
public class BookController : ControllerBase
{
    public readonly IBookService _bookService;

    public BookController(IBookService bookServiceFromBuilder)
    {
        _bookService = bookServiceFromBuilder;
    }

    [HttpGet("Books/{bookBarcode}")]
    public async Task<ActionResult<Book>> GetBookByBarcode(int bookBarcode)
    {
        return Ok( await _bookService.GetBookByBarcodeAsync(bookBarcode));
    }
}