using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class PhongService : PhongRepository
    {
        public void CapNhatPhong(Phong phong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE Phong SET tinhtrangphong = @tinhtrangphong ,loaiphong=@loaiphong WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tinhtrangphong", phong.tinhtrangphong);
                    command.Parameters.AddWithValue("@loaiphong", phong.loaiphong);
                    command.Parameters.AddWithValue("@id", phong.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Phong> GetAllPhong()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = "select * from Phong ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phong phong = new Phong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                idtang = Convert.ToInt32(reader["idtang"]),
                            };
                            phongs.Add(phong);
                        }
                    }
                }
                return phongs;
            }
        }
        public List<Phong> GetAllPhongTrangThai()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = "select * from Phong where tinhtrangphong = N'còn trống' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phong phong = new Phong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                idtang = Convert.ToInt32(reader["idtang"]),
                            };
                            phongs.Add(phong);
                        }
                    }
                }
                return phongs;
            }
        }

        public List<Phong> GetAllPhongIDTang(int idtang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string sql = "SELECT * FROM Phong where idtang = @idtang";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idtang", idtang);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phong phong = new Phong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                            };
                            phongs.Add(phong);
                        }
                    }
                }
                return phongs;
            }
        }

        public Phong GetPhongID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from Phong where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Phong phong = new Phong
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                            };
                            return phong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
