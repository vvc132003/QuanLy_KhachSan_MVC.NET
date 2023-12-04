using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
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
                                email = reader["email"].ToString(),
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

    }
}
