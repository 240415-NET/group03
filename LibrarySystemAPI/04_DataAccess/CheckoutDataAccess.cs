using Microsoft.EntityFrameworkCore;
using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public class CheckoutDataAccess : ICheckoutDataAccess
{
    private readonly LibrarySystemContext _checkoutContext;

    public CheckoutDataAccess(LibrarySystemContext checkoutContextFromBuilder)
    {
        _checkoutContext = checkoutContextFromBuilder;
    }

    public async Task<checkoutDTO> CreateNewCheckoutAsync(checkoutDTO newCheckoutFromService)
    {
        //Get the user object
        User? patron = await _checkoutContext.Users.SingleOrDefaultAsync(u => u.userId == newCheckoutFromService.userId);

        //get the book object
        Book? tome = await _checkoutContext.Books.SingleOrDefaultAsync(b => b.barcode == newCheckoutFromService.bookBarcode);

        Checkout checkoutToAdd = new(newCheckoutFromService, patron, tome);

        _checkoutContext.Add(checkoutToAdd);

        await _checkoutContext.SaveChangesAsync();

        return newCheckoutFromService;
    }

    public async Task<checkoutDTO> booksAvailableForCheckoutAsync()
    {
                List<Checkout> booksAvailable = await _checkoutContext.Checkouts.Select(u => u)
                    .Include(u => u.checkoutBook)
                    .Include(u=> u.checkoutUser)
                    .ToListAsync();
                booksAvailable.ForEach(x=>Console.WriteLine($"{x.checkoutId} {x.checkoutBook.barcode} {x.checkoutBook.author} {x.checkoutBook.genre} {x.status}"));

                List<Book> booksAvailable1 = await _checkoutContext.Books.Select(x=>x).Where(x=>x.genre=="Fantasy").ToListAsync();
                booksAvailable1.ForEach(x=>Console.WriteLine($"{x.author} {x.barcode} {x.title}"));
        return null;
    }
    
}