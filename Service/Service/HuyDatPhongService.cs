using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
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
        public List<HuyDatPhong> GetAllHuyDatPhongDescNgayHuy()
        {
            List<HuyDatPhong> huyDatPhongs = new List<HuyDatPhong>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM HuyDatPhong ORDER BY ngayhuy DESC";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HuyDatPhong huyDatPhong = new HuyDatPhong();
                            huyDatPhong.ngayhuy = reader.GetDateTime("ngayhuy");
                            huyDatPhong.lydo = reader.GetString("lydo");
                            huyDatPhong.iddatphong = reader.GetInt32("iddatphong");
                            huyDatPhong.idnhanvien = reader.GetInt32("idnhanvien");

                            huyDatPhongs.Add(huyDatPhong);
                        }
                    }
                }
            }

            return huyDatPhongs;
        }
    }
}
