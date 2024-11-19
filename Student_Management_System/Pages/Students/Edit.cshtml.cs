using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static Student_Management_System.Pages.Students.IndexModel;

namespace Student_Management_System.Pages.Students
{
    public class EditModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public String ErrorMsg = "";
        public String SuccessMsg = "";
       
            public void OnGet()
            {
               
            String id = Request.Query["id"];
            try
                {
                    String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentdb;Integrated Security=True;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        String sql = "SELECT * FROM student WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                        {

                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    studentInfo.id = reader.GetInt32(0).ToString(); 
                                    studentInfo.name = reader.GetString(1);
                                    studentInfo.email = reader.GetString(2);
                                    studentInfo.phone = reader.GetString(3);
                                    studentInfo.address = reader.GetString(4);
                                }
                                else
                                {
                                    // Handle case where no student is found with the id
                                    ErrorMsg = "Student with id " + id + " not found.";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }
            }
          public void OnPost()
        {
            studentInfo.id = Request.Form["id"];
            studentInfo.name = Request.Form["name"];
            studentInfo.email = Request.Form["email"];
            studentInfo.phone = Request.Form["phone"];
            studentInfo.address = Request.Form["address"];


            if (string.IsNullOrEmpty(studentInfo.name)
 || string.IsNullOrEmpty(studentInfo.email) ||
                string.IsNullOrEmpty(studentInfo.phone) || string.IsNullOrEmpty(studentInfo.address))
            {
                ErrorMsg = "All fields are required";
                return;
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentdb;Integrated Security=True;";

                //creating sql connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //sql qurey
                    string sql = "UPDATE student SET " +
                                 "name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";
                  

                    //create command
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@id", studentInfo.id);
                        command.Parameters.AddWithValue("@name", studentInfo.name);
                        command.Parameters.AddWithValue("@email", studentInfo.email);
                        command.Parameters.AddWithValue("@phone",studentInfo.phone);
                        command.Parameters.AddWithValue("@address", studentInfo.address);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return;
            }
            Response.Redirect("/Students/Index");
        }
    } 
}

    
