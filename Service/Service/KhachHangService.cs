using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class KhachHangService : KhachHangRepository
    {
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
        public void KhachHangDangKy(KhachHang khachHang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = @"INSERT INTO KhachHang (hovaten, sodienthoai, email, cccd, tinh, huyen, phuong, taikhoan, matkhau,  trangthai) 
                         VALUES (@hovaten, @sodienthoai, @email, @cccd, @tinh, @huyen, @phuong,  @trangthai, @matkhau , @taikhoan)";
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
                        trangthai = @trangthai WHERE id = @id";

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
