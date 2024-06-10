using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public interface IBookService
{
    public Task<Book> GetBookByBarcodeAsync(int bookBarcodeFromController);
}