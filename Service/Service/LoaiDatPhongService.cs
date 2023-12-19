using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class LoaiDatPhongService : LoaiDatPhongRepository
    {
        public List<LoaiDatPhong> GetAllLoaiDatPhong()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<LoaiDatPhong> loaiDatPhongs = new List<LoaiDatPhong>();
                connection.Open();
                string query = "select * from LoaiDatPhong";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LoaiDatPhong loaiDatPhong = new LoaiDatPhong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaidatphong = reader["loaidatphong"].ToString(),
                            };
                            loaiDatPhongs.Add(loaiDatPhong);
                        }
                    }
                }
                return loaiDatPhongs;
            }
        }

        public void ThemLoaiDatPhong(LoaiDatPhong loaiDatPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO LoaiDatPhong (loaidatphong) VALUES (@loaidatphong)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loaidatphong", loaiDatPhong.loaidatphong);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
