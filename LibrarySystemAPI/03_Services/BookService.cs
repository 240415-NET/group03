using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public class BookService : IBookService
{
    private readonly IBookDataAccess _bookDataAccess;

    public BookService(IBookDataAccess bookDataAccessFromBuilder)
    {
        _bookDataAccess = bookDataAccessFromBuilder;
    }

    public async Task<Book> GetBookByBarcodeAsync(int bookBarcodeFromController)
    {
        Book? foundBook = await _bookDataAccess.GetBookByBarcodeAsync(bookBarcodeFromController);
        return foundBook;
    }
}