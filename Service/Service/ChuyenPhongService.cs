using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class ChuyenPhongService : ChuyenPhongRepository
    {
        public void ThemChuyenPhong(ChuyenPhong chuyenPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string insertQuery = "INSERT INTO ChuyenPhong (ngaychuyen, lydo, idkhachhang, idnhanvien, idphongcu, idphongmoi) " +
                                     " VALUES (@ngaychuyen, @lydo, @idkhachhang, @idnhanvien, @idphongcu, @idphongmoi)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ngaychuyen", chuyenPhong.ngaychuyen);
                    command.Parameters.AddWithValue("@lydo", chuyenPhong.lydo);
                    command.Parameters.AddWithValue("@idkhachhang", chuyenPhong.idkhachhang);
                    command.Parameters.AddWithValue("@idnhanvien", chuyenPhong.idnhanvien);
                    command.Parameters.AddWithValue("@idphongcu", chuyenPhong.idphongcu);
                    command.Parameters.AddWithValue("@idphongmoi", chuyenPhong.idphongmoi);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
