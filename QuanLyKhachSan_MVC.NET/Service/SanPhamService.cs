using DocumentFormat.OpenXml.Spreadsheet;
using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class SanPhamService : SanPhamRepository
    {
        public void CapNhatSanPham(SanPham sanPham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "UPDATE SanPham SET tensanpham = @tensanpham, mota = @mota, soluongcon = @soluongcon, " +
                    "image = @image, trangthai = @trangthai WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tensanpham", sanPham.tensanpham);
                    command.Parameters.AddWithValue("@mota", sanPham.mota);
                    command.Parameters.AddWithValue("@soluongcon", sanPham.soluongcon);
                    command.Parameters.AddWithValue("@image", sanPham.image);
                    command.Parameters.AddWithValue("@trangthai", sanPham.trangthai);
                    command.Parameters.AddWithValue("@id", sanPham.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<SanPham> GetAllSanPham()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<SanPham> sanPhams = new List<SanPham>();
                connection.Open();
                string sql = "SELECT * FROM SanPham";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SanPham sanPham = new SanPham()
                            {
                                id = (int)reader["id"],
                                tensanpham = reader["tensanpham"].ToString(),
                                mota = reader["mota"].ToString(),
                                soluongcon = (int)reader["soluongcon"],
                                image = reader["image"].ToString(),
                                trangthai = reader["trangthai"].ToString()
                            };
                            sanPhams.Add(sanPham);
                        }
                    }
                }
                return sanPhams;
            }
        }

        public SanPham GetSanPhamByID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM SanPham where id = @id ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SanPham sanPham = new SanPham()
                            {
                                id = (int)reader["id"],
                                tensanpham = reader["tensanpham"].ToString(),
                                mota = reader["mota"].ToString(),
                                soluongcon = (int)reader["soluongcon"],
                                image = reader["image"].ToString(),
                                trangthai = reader["trangthai"].ToString()
                            };
                            return sanPham;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemSanPham(SanPham sanPham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO SanPham (tensanpham, mota, soluongcon, image, trangthai) " +
                                     "VALUES (@tensanpham, @mota, @soluongcon, @image, @trangthai)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tensanpham", sanPham.tensanpham);
                    command.Parameters.AddWithValue("@mota", sanPham.mota);
                    command.Parameters.AddWithValue("@soluongcon", sanPham.soluongcon);
                    command.Parameters.AddWithValue("@image", sanPham.image);
                    command.Parameters.AddWithValue("@trangthai", sanPham.trangthai);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaSanPham(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "DELETE FROM SanPham WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
