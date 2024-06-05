using LibrarySystem.Model;
using LibrarySystem.Controller;

namespace LibrarySystem;

class Menu
{
    static void Main(string[] args)
    {
        ScreenOne();
    } 
    
    public static void ScreenOne() {
        BookController.ClearCart(); 
        switch (MenuOneSelection() )
        {
            case 0: //Exit
                break;
            case 1: //Login
                MenuUserLogin(); 
                ScreenTwo();
                break;
            case 2: //Create Account
                MenuUserCreation();
                ScreenTwo();
                break;
                
        };

    }

      public static void ScreenTwo() {
        
        switch ( MenuTwoSelection())
        {
            case 0: //Logout & Exit
               ScreenOne();
                break;
            case 1:  //Search by Title
                BookSearchSelection();
                break;
            case 2: //View Selected Cart for Checkout 
                CurrentCheckedOutCart(); 
                break;
            case 3: //View Currently Checked out Books for Checkin
                CheckedOutBooks();
                break;            
        };

    }
     

    public static int MenuOneSelection() {
        string stringSelection = "";
        do{
            Console.WriteLine(@"
            Welcome to the Library!
            Menu Option  
             0 - Exit
             1 - Login
             2 - Create Account
             " );
           stringSelection = Console.ReadLine() ?? "";
        } 
        while (!ValidateMenuInput(stringSelection, 0, 2 ));
        int intSelection = Convert.ToInt16(stringSelection);
        return intSelection;
    }

     public static int MenuTwoSelection() {
        string stringSelection = "";
        do{
            Console.WriteLine(@"
            Welcome {0} to the library!
            Menu Option   
            0 - Logout
            1 - Search by Title
            2 - View Selected Cart for Checkout 
            3 - View Currently Checked out Books for Checkin
             " , UserController.GetCurrentUserName());
            stringSelection = Console.ReadLine() ?? "";
        } 
        while (!ValidateMenuInput(stringSelection, 0, 4 ));
        int intSelection = Convert.ToInt16(stringSelection);
        return intSelection;
            
    }

     public static void MenuUserCreation() {

        string stringSelection = "";
        bool repeat = true;
        do{
                Console.WriteLine("Please enter a new Username:");
                string userInput = Console.ReadLine() ?? "";
                userInput = userInput.Trim();

                if(String.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Username cannot be blank. Please enter another username: ");
                    
                }else if(UserController.UserExists(userInput))
                {
                    Console.WriteLine("Username {0} already exists. Please try another username:", userInput);
                   
                } else { 
                    UserController.CreateUser(userInput);
                    repeat = false;
                }
        }while (repeat);
     }

      public static void MenuUserLogin() {
            
        string stringSelection = "";
        bool repeat = true;
        do{
                Console.WriteLine("Please enter your login username:" );
                string userInput = Console.ReadLine() ?? "";
                userInput = userInput.Trim();
                if(String.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Username cannot be blank. Please enter another username: ");
                   
                }else if(!UserController.UserExists(userInput))
                {
                    Console.WriteLine("Username {0} does not exist. Please try another username:", userInput);
                    
                } else { 
                    UserController.ReturnUser(userInput);
                    repeat = false;
                }

            } 
        while (repeat);      
     }

     public static void BookSearchSelection(){
       
      
        string[] inputs;
        do{

            Console.WriteLine("Please enter your book search (cannot be null or empty) by Title:");
            inputs = Console.ReadLine().Split(' ');
            
        }while (String.IsNullOrEmpty(inputs[0]));

        List<string> inputsCleaned = new List<String>();
        for(int i = 0; i < inputs.Length; i++){
            if(inputs[i].ToLower() != "the" && inputs[i].ToLower() != "of" && inputs[i].ToLower() != "be" && inputs[i].ToLower() != "to" && inputs[i].ToLower() != "and" && inputs[i].ToLower() != "a" && inputs[i].ToLower() != "an" && inputs[i].ToLower() != "i"  ){
          
                //Console.WriteLine(inputs[i].ToLower());
                inputsCleaned.Add(inputs[i].ToLower());
            }
        }

        string whereClause = "";
        for(int i = 0; i < inputsCleaned.Count; i++){
                if (i == 0) {
                whereClause += "where title like '%" + inputsCleaned[i] + "%' ";
                } else if(i == inputsCleaned.Count-1){
                whereClause += "or title like '%" + inputsCleaned[i] + "%' ";
                } else {
                whereClause += "or title like '%" + inputsCleaned[i] + "%' ";
                }
        }
        //Console.WriteLine(whereClause);
        
        List<Book> bookResultList= BookController.BookSearch(whereClause);
     
        if(bookResultList.Count > 0){
             Console.WriteLine(@"
                         Select Book Option to Add to CART ");
               Console.WriteLine(@"Option 0: Return to Main Menu");
            foreach(Book book in bookResultList ){
               
                Console.WriteLine(@"Option " + book.index + ": " + book.title + " " + book.author );
            }
            
        string stringSelection = "";
        do{
            stringSelection = Console.ReadLine() ?? "";
        } 
        while (!ValidateMenuInput(stringSelection, 0, bookResultList.Count ));
        int intSelection = Convert.ToInt16(stringSelection);
            
        if(intSelection ==0){
             ScreenTwo();
        }  else { 
            foreach(Book book in bookResultList ){
                if(intSelection == book.index ) {
                    BookController.SetCart(book, UserController.GetCurrentUserName());
                    Console.WriteLine(@"
                             Added to CART " + book.index + " " + book.title + " " + book.author + "" );

                }
               
            }
        }

            ScreenTwo();
            
        } else {
            Console.WriteLine(@"Results: Sorry. No books match critera. Returning to Main Menu ");
            ScreenTwo();
        }
     }


     public static void CurrentCheckedOutCart () {
             List<Book> userCart  = BookController.GetCart();
             if(userCart.Count == 0){
                Console.WriteLine("No books selected in Cart");
                ScreenTwo();
             }else {
                foreach(Book bookincart in userCart){
                    Console.WriteLine(@"Cart: " + bookincart.title + " " + bookincart.author);
                }

                Console.WriteLine(@"
                Menu Option   
                0 - Return to Main Menu
                1 - Checkout 
                2 - Clear cart
                ");

                string stringSelection = "";
                do{
                    stringSelection = Console.ReadLine() ?? "";
                } 
                while (!ValidateMenuInput(stringSelection, 0, 2 ));
                int intSelection = Convert.ToInt16(stringSelection);

                 switch (intSelection)
                {
                    case 0: 
                        ScreenTwo();
                        break;
                    case 1:  
                        BookController.Checkout();
                        ScreenTwo();
                        break;
                    case 2: 
                        BookController.ClearCart(); 
                        ScreenTwo();
                        break;             
                };

             }
     }

     public static void CheckedOutBooks(){
        string userCheckedout = BookController.QueryCheckedOutBooks();
       
        if(userCheckedout == "") {
            Console.WriteLine(@"No books are needed to checkin for {0}", UserController.GetCurrentUserName());
            ScreenTwo();
        } else {
            Console.WriteLine(@"
                Option 0: Return to Main Menu " + userCheckedout);

            string stringSelection = "";
            do{
                    Console.WriteLine("Select Option to Check In a book:");
                    stringSelection = Console.ReadLine() ?? "";
                } 
            while (!ValidateMenuInput(stringSelection, 0, BookController.GetOp() ));
            int intSelection = Convert.ToInt16(stringSelection);
        
            if(intSelection ==0){
                ScreenTwo();
            } else {
              BookController.CheckIn(intSelection);
            }
        }
        
     }

     public static bool ValidateMenuInput(string MenuSelection, int optionSizeStart, int optionSizeEnd)
        {
            try
            {
                if (Convert.ToInt16(MenuSelection) >= optionSizeStart && Convert.ToInt16(MenuSelection) <= optionSizeEnd)
                {
                    return true;
                }
                else if (String.IsNullOrEmpty(MenuSelection)){
                    Console.WriteLine($"Menu selection cannot be null or empty.");
                    return false;
                } else
                {
                    Console.WriteLine($"Menu selection must be between {optionSizeStart} and {optionSizeEnd}.");
                    return false;
                }
            }
            catch (Exception excp)
            {
                Console.WriteLine($"Error detected {excp.Message}");
                return false;
            }
        }
}

