using CharityWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CharityWeb.DAL
{
    public class NursingHomeDAL
    {
        /*private string connectionString = WebConfigurationManager.ConnectionStrings["NursingHomesDB"].ConnectionString;*/
        private string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public List<NursingHome> GetAllNursingHomes()
        {
            List<NursingHome> nursingHomes = new List<NursingHome>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM NursingHomes", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NursingHome home = new NursingHome
                    {
                        ID = (int)reader["ID"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Price = (decimal)reader["Price"],
                        ImageUrl = (string)reader["ImageUrl"]
                    };
                    nursingHomes.Add(home);
                }
            }

            return nursingHomes;
        }
        public NursingHome GetNursingHomeById(int id)
        {
            NursingHome nursingHome = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM NursingHomes WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    nursingHome = new NursingHome
                    {
                        ID = (int)reader["ID"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Price = (decimal)reader["Price"],
                        ImageUrl = (string)reader["ImageUrl"]
                    };
                }
            }

            return nursingHome;
        }
    }
}