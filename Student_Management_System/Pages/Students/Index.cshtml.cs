using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Student_Management_System.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<StudentInfo> ListStudents = new List<StudentInfo>(); // Initialize the list

        public void OnGet()
        {
            try
            {
                // Connect to database

                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentdb;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create query
                    String sql = "SELECT * FROM student";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            ListStudents.Clear(); // Clear the list before filling it (optional)
                            while (reader.Read())
                            {
                                StudentInfo studentInfo = new StudentInfo();
                                studentInfo.id = reader.GetInt32(0).ToString(); // Use PascalCase for properties
                                studentInfo.name = reader.GetString(1);
                                studentInfo.email = reader.GetString(2);
                                studentInfo.phone = reader.GetString(3);
                                studentInfo.address = reader.GetString(4);
                                studentInfo.createdAt = reader.GetDateTime(5).ToString();

                                ListStudents.Add(studentInfo);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error connecting to database: " + ex.Message);
                // Log the error for investigation and handle it appropriately
            }
        }

        public class StudentInfo
        {
            public string id;  // Make id nullable (optional, depending on your data)
            public string name;
            public string email; 
            public string phone ;
            public string address; 
            public string createdAt; 
        }
    }
}