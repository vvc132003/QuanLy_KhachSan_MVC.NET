using ketnoicsdllan1;
using Model.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class IconService : IconRepository
    {
        public List<Icon> GetIconList()
        {
            List<Icon> iconList = new List<Icon>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Icon";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Icon icon = new Icon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Icons = reader["Icons"].ToString(),
                            ThoiGianThem = Convert.ToDateTime(reader["ThoiGianThem"])
                        };

                        iconList.Add(icon);
                    }
                }
            }

            return iconList;
        }
        public Icon GetIconById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Icon WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Icon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Icons = reader["Icons"].ToString(),
                            ThoiGianThem = Convert.ToDateTime(reader["ThoiGianThem"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }
        public void AddIcon(Icon icon)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO Icon (Icons, ThoiGianThem) VALUES (@Icons, @ThoiGianThem)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Icons", icon.Icons);
                command.Parameters.AddWithValue("@ThoiGianThem", icon.ThoiGianThem);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateIcon(Icon icon)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE Icon SET Icons = @Icons, ThoiGianThem = @ThoiGianThem WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", icon.Id);
                command.Parameters.AddWithValue("@Icons", icon.Icons);
                command.Parameters.AddWithValue("@ThoiGianThem", icon.ThoiGianThem);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteIcon(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM Icon WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
