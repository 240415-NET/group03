using Microsoft.EntityFrameworkCore;
using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public class BookDataAccess : IBookDataAccess
{
    private readonly LibrarySystemContext bookContext;

    public BookDataAccess(LibrarySystemContext bookContextFromBuilder)
    {
        bookContext = bookContextFromBuilder;
    }

    public async Task<Book?> GetBookByBarcodeAsync(int bookBarcodeFromService)
    {
        Book? foundBook = await bookContext.Books.SingleOrDefaultAsync(bk => bk.barcode == bookBarcodeFromService);

        return foundBook;
    }
}