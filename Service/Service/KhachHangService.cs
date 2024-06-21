using Auth0.ManagementApi.Models;
using ketnoicsdllan1;
using Microsoft.AspNetCore.Identity;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class KhachHangService : KhachHangRepository
    {

        private readonly PasswordHasher<KhachHang> _passwordHasher;
        public KhachHangService(PasswordHasher<KhachHang> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null,password);
        }

      
        public PasswordVerificationResult VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }


        public List<KhachHang> GetAllKhachHang()
        {
            List<KhachHang> listKhachHang = new List<KhachHang>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang ORDER BY id DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachHang khachHang = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                idtaikhoangoogle = reader["idtaikhoangoogle"].ToString(),

                            };
                            listKhachHang.Add(khachHang);
                        }
                    }
                }
            }
            return listKhachHang;
        }

        public KhachHang GetKhachHangCCCD(string cccd)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where cccd = @cccd";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cccd", cccd);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                            };
                            return KhachHangs;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public KhachHang GetKhachHangbyid(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                idtaikhoangoogle = reader["idtaikhoangoogle"].ToString(),

                            };
                            return KhachHangs;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public KhachHang GetKhachHangbyemail(string email)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                // Adjust the query to use proper parameter handling to avoid SQL injection
                string query = "SELECT * FROM KhachHang WHERE LTRIM(RTRIM(LOWER(email))) = LTRIM(RTRIM(LOWER(@Email)))";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ensure the parameter value is trimmed and lowercased
                    command.Parameters.AddWithValue("@Email", email.Trim().ToLower());

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create and populate the KhachHang object from the data reader
                            KhachHang khachHang = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                idtaikhoangoogle = reader["idtaikhoangoogle"].ToString(),
                            };
                            return khachHang;
                        }
                        else
                        {
                            // Log that no results were found
                            Console.WriteLine($"No customer found with the provided email: {email}");
                            return null;
                        }
                    }
                }
            }
        }


        public void ThemKhachHang(KhachHang khachHang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = @"INSERT INTO KhachHang (hovaten, sodienthoai, email, cccd, tinh, huyen, phuong, trangthai) 
                         VALUES (@hovaten, @sodienthoai, @email, @cccd, @tinh, @huyen, @phuong,  @trangthai)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@hovaten", khachHang.hovaten);
                command.Parameters.AddWithValue("@sodienthoai", khachHang.sodienthoai);
                command.Parameters.AddWithValue("@email", khachHang.email);
                command.Parameters.AddWithValue("@cccd", khachHang.cccd);
                command.Parameters.AddWithValue("@tinh", khachHang.tinh);
                command.Parameters.AddWithValue("@huyen", khachHang.huyen);
                command.Parameters.AddWithValue("@phuong", khachHang.phuong);
                command.Parameters.AddWithValue("@trangthai", khachHang.trangthai);
                command.ExecuteNonQuery();
            }
        }
        public void ThemKhachHangGoogle(KhachHang khachHang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = @"INSERT INTO KhachHang (hovaten, email, idtaikhoangoogle) 
                         VALUES (@hovaten, @email, @idtaikhoangoogle)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@hovaten", khachHang.hovaten);
                command.Parameters.AddWithValue("@email", khachHang.email);
                command.Parameters.AddWithValue("@idtaikhoangoogle", khachHang.idtaikhoangoogle);
                command.ExecuteNonQuery();
            }
        }
        public KhachHang GetKhachHangbyidtaikhoangoogle(string idtaikhoangoogle)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where idtaikhoangoogle = @idtaikhoangoogle";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idtaikhoangoogle", idtaikhoangoogle);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                idtaikhoangoogle = reader["idtaikhoangoogle"].ToString(),

                            };
                            return KhachHangs;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public void KhachHangDangKy(KhachHang khachHang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = @"INSERT INTO KhachHang (hovaten, sodienthoai, email, cccd, tinh, huyen, phuong, taikhoan, matkhau,  trangthai) 
                         VALUES (@hovaten, @sodienthoai, @email, @cccd, @tinh, @huyen, @phuong, @taikhoan, @matkhau,  @trangthai)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@hovaten", khachHang.hovaten);
                command.Parameters.AddWithValue("@sodienthoai", khachHang.sodienthoai);
                command.Parameters.AddWithValue("@email", khachHang.email);
                command.Parameters.AddWithValue("@cccd", khachHang.cccd);
                command.Parameters.AddWithValue("@tinh", khachHang.tinh);
                command.Parameters.AddWithValue("@huyen", khachHang.huyen);
                command.Parameters.AddWithValue("@phuong", khachHang.phuong);
                command.Parameters.AddWithValue("@trangthai", khachHang.trangthai);
                command.Parameters.AddWithValue("@taikhoan", khachHang.taikhoan);
                command.Parameters.AddWithValue("@matkhau", khachHang.matkhau);
                command.ExecuteNonQuery();
            }
        }
        public void CapNhatKhachHang(KhachHang khachHang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = @"UPDATE KhachHang SET hovaten = @hovaten, sodienthoai = @sodienthoai, 
                        email = @email, cccd = @cccd,
                        tinh = @tinh, huyen = @huyen, phuong = @phuong, taikhoan=@taikhoan, matkhau=@matkhau,
                        trangthai = @trangthai, idtaikhoangoogle = @idtaikhoangoogle WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@hovaten", khachHang.hovaten);
                command.Parameters.AddWithValue("@sodienthoai", khachHang.sodienthoai);
                command.Parameters.AddWithValue("@email", khachHang.email);
                command.Parameters.AddWithValue("@cccd", khachHang.cccd);
                command.Parameters.AddWithValue("@tinh", khachHang.tinh);
                command.Parameters.AddWithValue("@huyen", khachHang.huyen);
                command.Parameters.AddWithValue("@phuong", khachHang.phuong);
                command.Parameters.AddWithValue("@trangthai", khachHang.trangthai);
                command.Parameters.AddWithValue("@taikhoan", khachHang.taikhoan);
                command.Parameters.AddWithValue("@matkhau", khachHang.matkhau);
                command.Parameters.AddWithValue("@idtaikhoangoogle", khachHang.idtaikhoangoogle);
                command.Parameters.AddWithValue("@id", khachHang.id);
                command.ExecuteNonQuery();
            }
        }

        public KhachHang GetKhachHangDangNhap(string taikhoan, string matkhau)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where taikhoan = @taikhoan and matkhau=@matkhau ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    command.Parameters.AddWithValue("@matkhau", matkhau);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                            };
                            return KhachHangs;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public KhachHang GetKhachHangDangNhaps(string taikhoanoremail)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where taikhoan = @taikhoanoremail or email = @taikhoanoremail";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taikhoanoremail", taikhoanoremail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                            };
                            return KhachHangs;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public KhachHang GetKhachHangTaiKhoan(string taikhoan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachHang where taikhoan = @taikhoan  ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang KhachHangs = new KhachHang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                email = reader["email"].ToString(),
                                cccd = reader["cccd"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                            };
                            return KhachHangs;
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
