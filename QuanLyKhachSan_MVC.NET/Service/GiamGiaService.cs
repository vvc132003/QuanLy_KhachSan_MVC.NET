using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class GiamGiaService : GiamGiaRepository
    {
        public GiamGia GetGiamGiaBYIDKhachHang(int iddatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM GiamGia WHERE iddatphong = @iddatphong ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@iddatphong", iddatphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            GiamGia giamGia = new GiamGia()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                solandatphong = Convert.ToInt32(reader["solandatphong"]),
                                phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                                ngaythemgiamgia = Convert.ToDateTime(reader["ngaythemgiamgia"]),
                                idkhachhang = Convert.ToInt32(reader["idkhachhang"]),
                                idquydinh = Convert.ToInt32(reader["idquydinh"])
                            };
                            return giamGia;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemGiamGia(GiamGia giamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO GiamGia (solandatphong, phantramgiamgia, ngaythemgiamgia, idkhachhang, idquydinh, iddatphong) VALUES (@solandatphong, @phantramgiamgia, @ngaythemgiamgia, @idkhachhang, @idquydinh, @iddatphong)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@solandatphong", giamGia.solandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", giamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemgiamgia", giamGia.ngaythemgiamgia);
                command.Parameters.AddWithValue("@idkhachhang", giamGia.idkhachhang);
                command.Parameters.AddWithValue("@idquydinh", giamGia.idquydinh);
                command.Parameters.AddWithValue("@iddatphong", giamGia.iddatphong);
                command.ExecuteNonQuery();
            }
        }
    }
}
