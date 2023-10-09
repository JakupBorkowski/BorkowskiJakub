using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorkowskiJakub.Pages.Users
{
    public class EditModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
				String connectionString = "Data Source=KUBALAPTOP\\SQLEXPRESS;Initial Catalog=usersDB;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String usersTabDataQuery = "SELECT * FROM users WHERE id = @id";

					using (SqlCommand command = new SqlCommand(usersTabDataQuery, connection))
					{
						command.Parameters.AddWithValue("@id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                userInfo.id = "" + reader.GetInt32(0);
								userInfo.name = reader.GetString(1);
								userInfo.surname = reader.GetString(2);
								userInfo.birth_date = reader.GetDateTime(3).ToString("dd-MM-yyyy");
								userInfo.sex = reader.GetString(4);
								userInfo.atribute_name = reader.GetString(5);
								userInfo.atribute_value = reader.GetString(6);
							}
                        }
						
					}
				}
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
			userInfo.id = Request.Form["id"];
			userInfo.name = Request.Form["name"];
			userInfo.surname = Request.Form["surname"];
			userInfo.birth_date = Request.Form["birth_date"];
			userInfo.sex = Request.Form["sex"];
			userInfo.atribute_name = Request.Form["atribute_name"];
			userInfo.atribute_value = Request.Form["atribute_value"];

            

            if (userInfo.id.Length == 0 || userInfo.name.Length == 0 || userInfo.surname.Length == 0 || userInfo.birth_date.Length == 0 || userInfo.sex.Length == 0)
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
            else if (userInfo.surname.Length > 150)
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
				errorMessage = "Data urodzenia ma niedopowiedni format!";
				return;
			}

			try
			{
				String connectionString = "Data Source=KUBALAPTOP\\SQLEXPRESS;Initial Catalog=usersDB;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String usersTabDataQuery = "UPDATE users SET name=@name, surname=@surname, birth_date=@birth_date, sex=@sex, atribute_name=@atribute_name, atribute_value=@atribute_value WHERE id = @id";
					
					using (SqlCommand command = new SqlCommand(usersTabDataQuery, connection))
					{
						command.Parameters.AddWithValue("@id", userInfo.id);
						command.Parameters.AddWithValue("@name", userInfo.name);
						command.Parameters.AddWithValue("@surname", userInfo.surname);
						command.Parameters.AddWithValue("@birth_date", DateTime.Parse(userInfo.birth_date));
						command.Parameters.AddWithValue("@sex", userInfo.sex);
						command.Parameters.AddWithValue("@atribute_name", userInfo.atribute_name);
						command.Parameters.AddWithValue("@atribute_value", userInfo.atribute_value);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) 
			{ 
				errorMessage = ex.Message;
			}

			Response.Redirect("/Users/Index");
		}


    }
}
