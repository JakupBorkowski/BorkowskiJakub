using System.Net.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BorkowskiJakub.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
		public string succesMessage = "";

		public void OnGet()
        {
        }

        public void OnPost() 
        {
            userInfo.name = Request.Form["name"];
            userInfo.surname = Request.Form["surname"];
            userInfo.birth_date = Request.Form["birth_date"];
            userInfo.sex = Request.Form["sex"];
            userInfo.atribute_name = Request.Form["atribute_name"];
            userInfo.atribute_value = Request.Form["atribute_value"];

            

            if (userInfo.name.Length == 0 || userInfo.surname.Length == 0 || userInfo.birth_date.Length == 0 || userInfo.sex.Length == 0)
            {
                errorMessage = "Wszystkie pola musz¹ zostaæ wype³nione!";
                return;
            }
            if (userInfo.name.Length > 50 && userInfo.surname.Length > 150)
            {
                errorMessage = "Imiê i nazwisko jest za d³ugie!";
                return;
            }
            else if (userInfo.name.Length > 50)
            {
                errorMessage = "Imiê jest za d³ugie!";
                return;
            }
            else if(userInfo.surname.Length >150)
            {
                errorMessage = "Nazwisko jest za d³ugie!";
                return;
            }
			DateTime temp;
			if (DateTime.TryParse(userInfo.birth_date, out temp))
			{
				DateTime userBirthDate = DateTime.Parse(userInfo.birth_date);
				DateTime actualDate = DateTime.Now;
				if (userBirthDate > actualDate)
				{
					errorMessage = "Data urodzenia nie mo¿e byæ z przysz³oœci!";
					return;
				}
			}
            else
            {
                errorMessage = "Data urodzenia ma nieodpowiedni format!";
                return;
            }
			try
            {
				String connectionString = "Data Source=KUBALAPTOP\\SQLEXPRESS;Initial Catalog=usersDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
					String usersTabDataQuery = "INSERT INTO users (name, surname, birth_date, sex, atribute_name, atribute_value) VALUES (@name, @surname, @birth_date, @sex, @atribute_name, @atribute_value)";
                    
                    using (SqlCommand command = new SqlCommand(usersTabDataQuery,connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@surname",userInfo.surname);
                        command.Parameters.AddWithValue("@birth_date", DateTime.Parse(userInfo.birth_date));
                        command.Parameters.AddWithValue("@sex",userInfo.sex);
                        command.Parameters.AddWithValue("@atribute_name", userInfo.atribute_name);
                        command.Parameters.AddWithValue("@atribute_value",userInfo.atribute_value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userInfo.name = ""; userInfo.surname = ""; userInfo.birth_date= ""; userInfo.sex = ""; userInfo.atribute_name = ""; userInfo.atribute_value = "";
            succesMessage = "U¿ytkownik dodany poprawnie.";

            Response.Redirect("/Users/Index");

		}    
    }
}
