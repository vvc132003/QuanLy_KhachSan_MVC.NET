using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class HuyDatPhongService : HuyDatPhongRepository
    {
        public void ThemHuyDatPhong(HuyDatPhong huyDatPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO HuyDatPhong (ngayhuy,lydo,iddatphong,idnhanvien) VALUES (@ngayhuy,@lydo,@iddatphong,@idnhanvien)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngayhuy", huyDatPhong.ngayhuy);
                    command.Parameters.AddWithValue("@lydo", huyDatPhong.lydo);
                    command.Parameters.AddWithValue("@iddatphong", huyDatPhong.iddatphong);
                    command.Parameters.AddWithValue("@idnhanvien", huyDatPhong.idnhanvien);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
