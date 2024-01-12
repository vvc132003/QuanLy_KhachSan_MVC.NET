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
    public class TinNhanIconService : TinNhanIconRepository
    {
        public List<TinNhanIcon> GetTinNhanIconList()
        {
            List<TinNhanIcon> tinNhanIconList = new List<TinNhanIcon>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM TinNhanIcon";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TinNhanIcon tinNhanIcon = new TinNhanIcon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ThoiGianNhan = Convert.ToDateTime(reader["ThoiGianNhan"]),
                            TinNhanId = Convert.ToInt32(reader["TinNhanId"]),
                            IconId = Convert.ToInt32(reader["IconId"])
                        };

                        tinNhanIconList.Add(tinNhanIcon);
                    }
                }
            }

            return tinNhanIconList;
        }

        public TinNhanIcon GetTinNhanIconById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM TinNhanIcon WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TinNhanIcon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ThoiGianNhan = Convert.ToDateTime(reader["ThoiGianNhan"]),
                            TinNhanId = Convert.ToInt32(reader["TinNhanId"]),
                            IconId = Convert.ToInt32(reader["IconId"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }

        public void AddTinNhanIcon(TinNhanIcon tinNhanIcon)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO TinNhanIcon (ThoiGianNhan, TinNhanId, IconId) " +
                               "VALUES (@ThoiGianNhan, @TinNhanId, @IconId)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ThoiGianNhan", tinNhanIcon.ThoiGianNhan);
                command.Parameters.AddWithValue("@TinNhanId", tinNhanIcon.TinNhanId);
                command.Parameters.AddWithValue("@IconId", tinNhanIcon.IconId);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateTinNhanIcon(TinNhanIcon tinNhanIcon)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE TinNhanIcon SET ThoiGianNhan = @ThoiGianNhan, " +
                               "TinNhanId = @TinNhanId, IconId = @IconId WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", tinNhanIcon.Id);
                command.Parameters.AddWithValue("@ThoiGianNhan", tinNhanIcon.ThoiGianNhan);
                command.Parameters.AddWithValue("@TinNhanId", tinNhanIcon.TinNhanId);
                command.Parameters.AddWithValue("@IconId", tinNhanIcon.IconId);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteTinNhanIcon(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM TinNhanIcon WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
