using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class TraPhongService : TraPhongRepository
    {
        public void ThemTraPhong(TraPhong traPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO TraPhong (ngaytra, idnhanvien, iddatphong) VALUES (@ngaytra, @idnhanvien, @iddatphong);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ngaytra", traPhong.ngaytra);
                    command.Parameters.AddWithValue("@idnhanvien", traPhong.idnhanvien);
                    command.Parameters.AddWithValue("@iddatphong", traPhong.iddatphong);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
