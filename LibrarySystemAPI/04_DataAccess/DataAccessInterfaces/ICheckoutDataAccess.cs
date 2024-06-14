using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public interface ICheckoutDataAccess
{
    public Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromService);

    public Task<List<Book>> booksAvailableForCheckoutAsync();

    public Task<List<Checkout>> GetCheckedOutBooksbyUserIdAsync(Guid userIdFromService);

}