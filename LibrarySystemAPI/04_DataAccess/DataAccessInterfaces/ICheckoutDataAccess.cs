using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public interface ICheckoutDataAccess
{
    public Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromService);
    public Task<checkoutDTO> booksAvailableForCheckoutAsync();
}