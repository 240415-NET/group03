using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public interface ICheckoutService
{
    public Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromController);
    public Task<checkoutDTO> BooksAvailableForCheckoutAsync(checkoutDTO newCheckoutFromController);
}