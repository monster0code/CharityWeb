using CharityWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CharityWeb.DAL
{
    public class ActivityDAL
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public List<ActivityModels> GetAllActivity()
        {
            List<ActivityModels> activity = new List<ActivityModels>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Activities", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ActivityModels home = new ActivityModels
                    {
                        ID = (int)reader["ID"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Time = (string)reader["Time"],
                        Info = (string)reader["Info"],
                        ImageUrl = (string)reader["ImageUrl"]
                    };
                    activity.Add(home);
                }
            }

            return activity;
        }
        public ActivityModels GetActivityById(int id)
        {
            ActivityModels activityId = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Activities WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    activityId = new ActivityModels
                    {
                        ID = (int)reader["ID"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Time = (string)reader["Time"],
                        Info = (string)reader["Info"],
                        ImageUrl = (string)reader["ImageUrl"]
                    };
                }
            }

            return activityId;
        }
    }
}