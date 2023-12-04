using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class ChucVuService : ChucVuRepository
    {
        public void CapNhatChucVu(ChucVu chucVu)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ChucVu SET tenchucvu = @tenchucvu   WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenchucvu", chucVu.tenchucvu);
                    command.Parameters.AddWithValue("@id", chucVu.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ChucVu> GetAllChucVu()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ChucVu> chucVus = new List<ChucVu>();
                connection.Open();
                string query = "select * from ChucVu";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChucVu chucVu = new ChucVu()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenchucvu = reader["tenchucvu"].ToString(),
                            };
                            chucVus.Add(chucVu);
                        }
                    }
                }
                return chucVus;
            }
        }

        public ChucVu GetChucVuID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from ChucVu where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ChucVu chucVu = new ChucVu
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenchucvu = reader["tenchucvu"].ToString(),

                            };
                            return chucVu;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemChucVu(ChucVu chucVu)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ChucVu (tenchucvu) VALUES (@tenchucvu)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenchucvu", chucVu.tenchucvu);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaChucVu(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM ChucVu WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
