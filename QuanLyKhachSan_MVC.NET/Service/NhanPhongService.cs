using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class NhanPhongService : NhanPhongRepository
    {
        public void ThemNhanPhong(NhanPhong nhanphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO NhanPhong (ngaynhanphong, iddatphong, idnhanvien) VALUES (@ngaynhanphong, @iddatphong, @idnhanvien)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngaynhanphong", nhanphong.ngaynhanphong);
                    command.Parameters.AddWithValue("@iddatphong", nhanphong.iddatphong);
                    command.Parameters.AddWithValue("@idnhanvien", nhanphong.idnhanvien);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
