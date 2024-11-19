using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static Student_Management_System.Pages.Students.IndexModel;

namespace Student_Management_System.Pages.Students
{
    public class CreateModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public String ErrorMsg="";
        public String SuccessMsg = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
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
            //save the new student
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentdb;Integrated Security=True;";

                //creating sql connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //sql qurey
                    String sql = "INSERT INTO student " +
                        "(name,email,phone,address) VALUES" +
                        "(@name,@email,@phone,@address);";

                    //create command
                    using (SqlCommand command = new SqlCommand(sql, connection)) { 
                        command.Parameters.AddWithValue("@name", studentInfo.name);
                        command.Parameters.AddWithValue("@email", studentInfo.email);
                        command.Parameters.AddWithValue("@phone", studentInfo.phone);
                        command.Parameters.AddWithValue("@address", studentInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            
            } 
            catch (Exception ex) {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            studentInfo.name = "";
            studentInfo.email = "";
            studentInfo.phone = "";
            studentInfo.address = "";
            studentInfo.createdAt = "";

            SuccessMsg = "New student added";

            Response.Redirect("/Students/Index");
        }
    }
}
