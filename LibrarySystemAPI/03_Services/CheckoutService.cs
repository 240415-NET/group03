using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public class CheckoutService : ICheckoutService
{
    private readonly ICheckoutDataAccess _checkoutDataAccess;

    public CheckoutService(ICheckoutDataAccess checkoutDataAccessFromBuilder)
    {
        _checkoutDataAccess = checkoutDataAccessFromBuilder;
    }

    public async Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromController)
    {
        await _checkoutDataAccess.CreateNewCheckoutAsync(newCheckoutFromController);
        return newCheckoutFromController;
    }

    public async Task<List<Checkout>> GetCheckedOutBooksbyUserIdAsync(Guid userIdFromController)
    {
        return await _checkoutDataAccess.GetCheckedOutBooksbyUserIdAsync(userIdFromController);

    }

    public async Task<string> UpdateCheckinAsync(int barcodeFromController)
    {
        return await _checkoutDataAccess.UpdateCheckinInDBAsync(barcodeFromController);
    }
}