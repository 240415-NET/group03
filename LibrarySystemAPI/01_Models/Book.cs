using System.ComponentModel.DataAnnotations;
namespace LibrarySystem.API.Models;

//Added this File

public class Book {
    [Key]    
    public int barcode {get; set;}
    public string title {get; set;}
    public string author {get; set;}
    public string genre {get; set;}

    //public List<Checkout> bookCheckouts {get; set;} = new();


    public Book () {}

    public Book( int _barcode, string _title, string _author, string _genre){
        
        barcode = _barcode;
        title = _title;
        author = _author;
        genre = _genre;
    }
}