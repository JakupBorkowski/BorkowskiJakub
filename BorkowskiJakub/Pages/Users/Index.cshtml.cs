using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Newtonsoft.Json;
//using SwiftExcel;

namespace BorkowskiJakub.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<UserInfo> userList = new List<UserInfo>();
    
        	
        public void OnGet()
        {
			try
            {
                String connectionString = "Data Source=KUBALAPTOP\\SQLEXPRESS;Initial Catalog=usersDB;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String usersTabDataQuery = "SELECT * FROM users";
                    using (SqlCommand command = new SqlCommand(usersTabDataQuery, connection))
                    {
						using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.id = ""+reader.GetInt32(0);
                                userInfo.name = reader.GetString(1);
                                userInfo.surname = reader.GetString(2);
                                userInfo.birth_date =reader.GetDateTime(3).ToString("dd-MM-yyyy");
                                userInfo.sex = reader.GetString(4);
                                userInfo.atribute_name = reader.GetString(5);
                                userInfo.atribute_value = reader.GetString(6);

                             
                                userList.Add(userInfo);
                            }
                        }
                    }
                }
                            
            }
            catch(Exception ex)
            {
                Console.WriteLine("B£¥D");
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }


    public class UserInfo
    {
        public String id;
        public String name;
        public String surname;
        public String birth_date;
        public String sex;
        public String atribute_name;
        public String atribute_value;


    }
}
