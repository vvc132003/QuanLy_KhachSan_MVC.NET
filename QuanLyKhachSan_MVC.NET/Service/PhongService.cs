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
                string query = "UPDATE Phong SET sophong = @sophong,songuoi=@songuoi,idkhachsan=@idkhachsan, tinhtrangphong = @tinhtrangphong ,loaiphong=@loaiphong WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sophong", phong.sophong);
                    command.Parameters.AddWithValue("@songuoi", phong.songuoi);
                    command.Parameters.AddWithValue("@idkhachsan", phong.idkhachsan);
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
                                sophong = Convert.ToInt32(reader["sophong"]),
                                songuoi = Convert.ToInt32(reader["songuoi"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                idtang = Convert.ToInt32(reader["idtang"]),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
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
                                sophong = Convert.ToInt32(reader["sophong"]),
                                songuoi = Convert.ToInt32(reader["songuoi"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                idtang = Convert.ToInt32(reader["idtang"]),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),

                            };
                            phongs.Add(phong);
                        }
                    }
                }
                return phongs;
            }
        }

        public List<Phong> GetAllPhongIDTang(int idtang, int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string sql = "SELECT * FROM Phong where idtang = @idtang and idkhachsan=@idkhachsan";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idtang", idtang);
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phong phong = new Phong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                sophong = Convert.ToInt32(reader["sophong"]),
                                songuoi = Convert.ToInt32(reader["songuoi"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
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
                                sophong = Convert.ToInt32(reader["sophong"]),
                                songuoi = Convert.ToInt32(reader["songuoi"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),

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

        public void ThemPhong(Phong phong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO Phong (sophong,songuoi,loaiphong,tinhtrangphong,giatientheogio,giatientheongay,idtang,idkhachsan) " +
                    " VALUES (@sophong,@songuoi,@loaiphong,@tinhtrangphong,@giatientheogio,@giatientheongay,@idtang,@idkhachsan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sophong", phong.sophong);
                    command.Parameters.AddWithValue("@songuoi", phong.songuoi);
                    command.Parameters.AddWithValue("@tinhtrangphong", phong.tinhtrangphong);
                    command.Parameters.AddWithValue("@loaiphong", phong.loaiphong);
                    command.Parameters.AddWithValue("@giatientheogio", phong.giatientheogio);
                    command.Parameters.AddWithValue("@giatientheongay", phong.giatientheongay);
                    command.Parameters.AddWithValue("@idkhachsan", phong.idkhachsan);
                    command.Parameters.AddWithValue("@idtang", phong.idtang);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}