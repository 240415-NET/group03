using LibrarySystem.Model;
using LibrarySystem.DataAccess;
using System.Data.SqlClient;
namespace LibrarySystem.Controller;


public class BookController
{
  public static List<Book> cart = new List<Book>();
  public static string lastuser = "";
  public static Dictionary<int, int> checkinOptions = new Dictionary<int, int>();
  public static int optionEnd = 0;
   
  public static List<Book> BookSearch(string userSearch ){
          Console.WriteLine($"Searching ...");
               string cmdText =
                    @"SELECT rank() over(order by title desc) as _index, barcode, title, author, genre FROM books " + userSearch;
          List<Book> returnBookSearch = new List<Book>();
          using SqlConnection connection = new SqlConnection(ADOconnect.ConnectionString());
          connection.Open();
          using SqlCommand cmd = new SqlCommand(cmdText, connection);
          using SqlDataReader reader = cmd.ExecuteReader();
          while(reader.Read()){
               //Console.WriteLine("pass");
               Book book = new Book( Convert.ToInt32(reader[0].ToString()), Convert.ToInt32(reader[1].ToString()), reader.GetString(2), reader.GetString(3), reader.GetString(4));
               returnBookSearch.Add(book);
          }
          return returnBookSearch;
     }

     public static void SetCart(Book bookcart, string username  ){
          if(lastuser == ""){
               lastuser = username;
               cart.Add(bookcart);
          } else if (lastuser == username){
              cart.Add(bookcart);
          } else if (lastuser != username) {
               lastuser=username;
               cart.Clear();
               cart.Add(bookcart);
          }
     }

     public static List<Book> GetCart(){
         return cart;
     }

     public static void ClearCart(){
         cart.Clear();
     }


     public static void Checkout(){
          Console.WriteLine($"Checking out ...");
               
          Guid userGuid = UserController.GetCurrentGuid();
          string cmdText = @"SELECT b.barcode, b.title, b.author FROM (select rank() over(partition by barcode order by control_ts desc) as rnk,* from checkoutStatus) c join books b on b.barcode = c.barcode where rnk=1 and checkedout_status = 'OUT' ";
          using SqlConnection connection = new SqlConnection(ADOconnect.ConnectionString());
          connection.Open();
          using SqlCommand cmd = new SqlCommand(cmdText, connection);
          using SqlDataReader reader = cmd.ExecuteReader();
          List<int> NOTsafeToCheckout = new List<int>(); 
          while(reader.Read()){
               Console.WriteLine("Book: " + reader.GetString(1) + " " + reader.GetString(2) + "is already checked out. Sorry!" );
               NOTsafeToCheckout.Add(Convert.ToInt32(reader[0].ToString()));
          }
         
          foreach (Book book in cart){
               
               if(NOTsafeToCheckout.Count == 0){
                   
                    string cmdInsert = @"INSERT INTO checkoutStatus (barcode, userguid, checkedout_status, duedate, control_ts) VALUES (@barcode, @userguid, @checkedout_status, @duedate, DEFAULT) " ;
                    using SqlCommand cmd2 = new SqlCommand(cmdInsert, connection);
                    cmd2.Parameters.AddWithValue("@barcode", book.barcode);
                    cmd2.Parameters.AddWithValue("@userguid",userGuid );
                    cmd2.Parameters.AddWithValue("@checkedout_status", "OUT");
                    cmd2.Parameters.AddWithValue("@duedate", DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"));
           
                    cmd2.ExecuteNonQuery();

                    Console.WriteLine("Checkout Completed for " + book.title +" " +book.author);
                    

               } else {
                         //Console.WriteLine(book.barcode);
                         if(!NOTsafeToCheckout.Exists(item => item == book.barcode)){
                              
                              string cmdInsert = @"INSERT INTO checkoutStatus (barcode, userguid, checkedout_status, duedate, control_ts) VALUES (@barcode, @userguid, @checkedout_status, @duedate, DEFAULT) " ;
                              using SqlCommand cmd3 = new SqlCommand(cmdInsert, connection);
                              cmd3.Parameters.AddWithValue("@barcode", book.barcode);
                              cmd3.Parameters.AddWithValue("@userguid",userGuid );
                              cmd3.Parameters.AddWithValue("@checkedout_status", "OUT");
                              cmd3.Parameters.AddWithValue("@duedate", DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"));
                    
                              cmd3.ExecuteNonQuery();
                              
                              Console.WriteLine("Checkout Completed for " + book.title +" " +book.author);
                             
                             
                               
                    }
               }
          }


          ClearCart();

     }

      public static string QueryCheckedOutBooks(){
           Guid userGuid = UserController.GetCurrentGuid();
          string cmdText =
               string.Format(@"SELECT rank() over(order by title desc) as _index, b.barcode,  b.title, b.author, c.checkedout_status, c.duedate  FROM (select rank() over(partition by barcode order by control_ts desc) as rnk,* from checkoutStatus) c join books b on b.barcode = c.barcode where rnk=1 and checkedout_status = 'OUT' and userguid = '{0}' ", userGuid);
          string returnUserChecked = "";
          Dictionary<int, int> checkinOptions2 = new Dictionary<int, int>();
          using SqlConnection connection = new SqlConnection(ADOconnect.ConnectionString());
          connection.Open();
          using SqlCommand cmd4 = new SqlCommand(cmdText, connection);
          using SqlDataReader reader4 = cmd4.ExecuteReader();
          int count =0;
          while(reader4.Read()){
               returnUserChecked += @" 
               Check-In Option " + Convert.ToInt32(reader4[0].ToString()) + ": " + reader4.GetString(2) + " "+ reader4.GetString(3) + " Checked Status-" + reader4.GetString(4) + " "+ "Due Date- " + reader4.GetString(5) + "";
               checkinOptions2.Add(Convert.ToInt32(reader4[0].ToString()), Convert.ToInt32(reader4[1].ToString()));
               count ++;
          }
          SetCheckinOp(checkinOptions2);
          SetOp(count);
          return returnUserChecked;
     }

     public static void SetCheckinOp(Dictionary<int, int> options){
          checkinOptions = options ;
     }

     public static Dictionary<int, int> GetCheckinOp(){
         return checkinOptions;
     }
     
     public static void SetOp(int options){
           optionEnd = options ;
     }

     public static int GetOp(){
         return optionEnd ;
     }

     public static void CheckIn(int choosenOption){
          Dictionary<int, int> checkinOps = GetCheckinOp();
          Guid userGuid = UserController.GetCurrentGuid();
          int checkoutBarcode = 0;
          foreach (KeyValuePair<int,int> op in checkinOps)
          {   
            if(choosenOption == Convert.ToInt32(op.Key)){   
               //Console.WriteLine(op.Key + " owns " + op.Value);
               checkoutBarcode = Convert.ToInt32(op.Value);
            }
          }
          using SqlConnection connection = new SqlConnection(ADOconnect.ConnectionString());
          connection.Open();
          string cmdInsert = @"INSERT INTO checkoutStatus (barcode, userguid, checkedout_status, duedate, control_ts) VALUES (@barcode, @userguid, @checkedout_status, null, DEFAULT) " ;
                              using SqlCommand cmd3 = new SqlCommand(cmdInsert, connection);
                              cmd3.Parameters.AddWithValue("@barcode", checkoutBarcode);
                              cmd3.Parameters.AddWithValue("@userguid",userGuid );
                              cmd3.Parameters.AddWithValue("@checkedout_status", "IN");
                    
                              cmd3.ExecuteNonQuery();
                              
                              Console.WriteLine("Checkin Completed");
          
     }
     
    
}