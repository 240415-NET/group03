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

    public async Task<List<Book>> booksAvailableForCheckoutAsync()
    {
                //retrieve the list of checked out books from the Checkouts table
                List<Checkout> booksCheckedOut = await _checkoutContext.Checkouts.Where(u => u.status=="out")
                    .Include(u => u.checkoutBook)
                    .Include(u=> u.checkoutUser)
                    .ToListAsync();
                //booksAvailable.ForEach(x=>Console.WriteLine($"{x.checkoutBook.barcode}-{x.status}"));

                //retrieve the list of all books from the Books table
                List<Book> booksAll = await _checkoutContext.Books.Select(x=>x).Where(x=>x.genre=="Fantasy").ToListAsync();
                booksAll.ForEach(x=>Console.WriteLine($"{x.barcode}-{x.title}-{x.author}-{x.genre}"));

                //get the barcodes from CheckOut that we need to exclude
                List<int> checkedOutBarCodes = booksCheckedOut.Select(d => d.checkoutBook.barcode).ToList();
                //checkedOutBarCodes.ForEach(x=>Console.WriteLine(x));

                //Exclude the barcodes found previously
                List<Book> booksAvailable = booksAll.Where(x => !checkedOutBarCodes.Contains(x.barcode)).ToList();
                //booksAvailable.ForEach(x=>Console.WriteLine($"{x.barcode}-{x.title}-{x.genre}"));

        return booksAvailable;
    }
    
}