using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class BoPhanService : BoPhanRepository
    {
        public BoPhan BoPhanGetID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from BoPhan where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            BoPhan boPhan = new BoPhan
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenbophan = reader["tenbophan"].ToString(),
                                mota = reader["mota"].ToString(),
                                image = reader["image"].ToString(),
                            };
                            return boPhan;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public void CapNhatBoPhan(BoPhan boPhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE BoPhan SET tenbophan = @tenbophan, mota = @mota, image = @image WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenbophan", boPhan.tenbophan);
                    command.Parameters.AddWithValue("@mota", boPhan.mota);
                    command.Parameters.AddWithValue("@image", boPhan.image);
                    command.Parameters.AddWithValue("@id", boPhan.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<BoPhan> GetALLBoPhan()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<BoPhan> boPhans = new List<BoPhan>();
                connection.Open();
                string sql = "SELECT * FROM BoPhan";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BoPhan boPhan = new BoPhan()
                            {
                                id = (int)reader["id"],
                                tenbophan = reader["tenbophan"].ToString(),
                                mota = reader["mota"].ToString(),
                                image = reader["image"].ToString()
                            };
                            boPhans.Add(boPhan);
                        }
                    }
                }
                return boPhans;
            }
        }

        public void ThemBoPhan(BoPhan boPhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO BoPhan (tenbophan,mota,image) VALUES (@tenbophan,@mota,@image)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenbophan", boPhan.tenbophan);
                    command.Parameters.AddWithValue("@mota", boPhan.mota);
                    command.Parameters.AddWithValue("@image", boPhan.image);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaBoPhan(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM BoPhan WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}