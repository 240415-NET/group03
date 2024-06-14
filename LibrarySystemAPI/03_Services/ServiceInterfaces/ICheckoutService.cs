using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public interface ICheckoutService
{
    public Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromController);
    public Task<List<Checkout>> GetCheckedOutBooksbyUserIdAsync(Guid userIdFromController);
    public Task<int> UpdateCheckinAsync(CheckinUpdateDTO checkinFromController);
}