namespace LibrarySystem.API.Models;

//Added this File
public class Book {
    
    public int barcode {get; set;}
    public string title {get; set;}
    public string author {get; set;}
    public string genre {get; set;}

    public Book () {}

    public Book( int _barcode, string _title, string _author, string _genre){
        index = _index;
        barcode = _barcode;
        title = _title;
        author = _author;
        genre = _genre;
    }
}