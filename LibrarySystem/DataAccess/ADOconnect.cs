
using System.Data.SqlClient;
using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
namespace LibrarySystem.DataAccess;

class ADOconnect{
    
    public static string filePath = @"C:\Users\A261982\OneDrive - Government Employees Insurance Company\revature\connectionString.txt";

    public static string connectionString = File.ReadAllText(filePath);

    public static string ConnectionString(){
        return connectionString;
    }
    //Move to Data Access
} 