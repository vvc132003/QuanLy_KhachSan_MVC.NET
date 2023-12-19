using DocumentFormat.OpenXml.Office2010.Excel;
using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class HopDongLaoDongService : HopDongLaoDongRepository
    {
        public void CapNhatHopDongLaoDong(HopDongLaoDong hopDongLaoDong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE HopDongLaoDong SET loaihopdong = @loaihopdong, ngaybatdau = @ngaybatdau, ngayketthuc = @ngayketthuc WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loaihopdong", hopDongLaoDong.loaihopdong);
                    command.Parameters.AddWithValue("@ngaybatdau", hopDongLaoDong.ngaybatdau);
                    command.Parameters.AddWithValue("@ngayketthuc", hopDongLaoDong.ngayketthuc);
                    command.Parameters.AddWithValue("@id", hopDongLaoDong.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public HopDongLaoDong GetHopDongLaoDongByID(int idnhanvien)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from HopDongLaoDong where idnhanvien = @idnhanvien";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnhanvien", idnhanvien);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            HopDongLaoDong hopDongLaoDong = new HopDongLaoDong
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaihopdong = reader["loaihopdong"].ToString(),
                                ngaybatdau = (DateTime)reader["ngaybatdau"],
                                ngayketthuc = (DateTime)reader["ngayketthuc"],
                            };
                            return hopDongLaoDong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemHopDongLaoDong(HopDongLaoDong hopDongLaoDong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO HopDongLaoDong (loaihopdong,ngaybatdau,ngayketthuc,idnhanvien) VALUES (@loaihopdong,@ngaybatdau,@ngayketthuc,@idnhanvien)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loaihopdong", hopDongLaoDong.loaihopdong);
                    command.Parameters.AddWithValue("@ngaybatdau", hopDongLaoDong.ngaybatdau);
                    command.Parameters.AddWithValue("@ngayketthuc", hopDongLaoDong.ngayketthuc);
                    command.Parameters.AddWithValue("@idnhanvien", hopDongLaoDong.idnhanvien);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
