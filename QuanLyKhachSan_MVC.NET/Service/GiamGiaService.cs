using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class GiamGiaService : GiamGiaRepository
    {
        public void ThemGiamGia(GiamGia giamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO GiamGia (solandatphong, phantramgiamgia, ngaythemgiamgia, idkhachhang, idquydinh) VALUES (@solandatphong, @phantramgiamgia, @ngaythemgiamgia, @idkhachhang, @idquydinh)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@solandatphong", giamGia.solandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", giamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemgiamgia", giamGia.ngaythemgiamgia);
                command.Parameters.AddWithValue("@idkhachhang", giamGia.idkhachhang);
                command.Parameters.AddWithValue("@idquydinh", giamGia.idquydinh);
                command.ExecuteNonQuery();
            }
        }
    }
}
