using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
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
        public List<Phong> GetAllPhongByIdKhachSan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = "select * from Phong where  idkhachsan=@idkhachsan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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
        public List<Phong> GetAllPhongSoPhong(string loaiphong, int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = "select * from Phong where  loaiphong=@loaiphong and idkhachsan=@idkhachsan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loaiphong", loaiphong);
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
        public List<string> GetAllLoaiPhongIdKhachSan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<string> loaiPhongs = new List<string>();
                connection.Open();
                string query = "SELECT DISTINCT loaiphong FROM Phong WHERE idkhachsan = @idkhachsan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string loaiPhong = reader["loaiphong"].ToString();
                            loaiPhongs.Add(loaiPhong);
                        }
                    }
                }
                return loaiPhongs;
            }
        }
        public List<int> GetAllSoNguoiLoaiPhong(string loaiphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<int> songuois = new List<int>();
                connection.Open();
                string query = "SELECT DISTINCT songuoi FROM Phong WHERE loaiphong = @loaiphong";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loaiphong", loaiphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int songuoi = Convert.ToInt32(reader["songuoi"]);
                            songuois.Add(songuoi);
                        }
                    }
                }
                return songuois;
            }
        }

        public List<Phong> GetAllPhongTrangThai(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = "select * from Phong where tinhtrangphong = N'còn trống' and idkhachsan=@idkhachsan ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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
        public List<Phong> GetAllPhongTrangThaiandidksloaiphongsonguoi(int idkhachsan, string loaiphong, int songuoi)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string query = @"
                                SELECT Phong.id, Phong.sophong, 
                                MAX(Phong.giatientheogio) AS giatientheogio, 
                                MAX(Phong.giatientheongay) AS giatientheongay, 
                                Phong.idkhachsan, Phong.loaiphong, Phong.songuoi
                                FROM DatPhong
                                LEFT JOIN Phong ON DatPhong.idphong = Phong.id
                                WHERE Phong.idkhachsan = @idkhachsan AND Phong.loaiphong = @loaiphong AND Phong.songuoi = @songuoi
                                GROUP BY Phong.id, Phong.sophong, Phong.idkhachsan, Phong.loaiphong, Phong.songuoi
                                HAVING COUNT(DatPhong.idphong) > 3 ;
                            ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    command.Parameters.AddWithValue("@loaiphong", loaiphong);
                    command.Parameters.AddWithValue("@songuoi", songuoi);
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


        public List<Phong> GetAllPhongByidKhachSanndlaoiphongandsonguoi(int idkhachsan, string loaiphong, int songuoi)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();

                // Xây dựng câu truy vấn dựa trên các điều kiện
                string query = "SELECT * FROM Phong WHERE idkhachsan = @idkhachsan";

                // Thêm điều kiện loại phòng nếu được chỉ định
                if (!string.IsNullOrEmpty(loaiphong))
                {
                    query += " AND loaiphong = @loaiphong";
                }

                // Thêm điều kiện số người nếu được chỉ định
                if (songuoi > 0)
                {
                    query += " AND songuoi = @songuoi";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);

                    // Thêm các tham số cho loại phòng và số người nếu chúng được chỉ định
                    if (!string.IsNullOrEmpty(loaiphong))
                    {
                        command.Parameters.AddWithValue("@loaiphong", loaiphong);
                    }
                    if (songuoi > 0)
                    {
                        command.Parameters.AddWithValue("@songuoi", songuoi);
                    }

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
        public Phong GetPhongIDKhachSan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from Phong where idkhachsan = @idkhachsan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
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
        public Phong GetPhongIDsophong(int id)
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

        public List<Phong> GetAllPhongIDKhachSan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Phong> phongs = new List<Phong>();
                connection.Open();
                string sql = "SELECT * FROM Phong where idkhachsan=@idkhachsan";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
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
    }
}