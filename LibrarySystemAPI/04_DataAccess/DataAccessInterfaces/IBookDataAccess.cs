using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public interface IBookDataAccess
{
    public Task<Book?> GetBookByBarcodeAsync(int bookBarcodeFromService);
}