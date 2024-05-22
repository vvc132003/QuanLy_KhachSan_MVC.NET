using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
using System.Data.SqlClient;

namespace Service
{
    public class LichSuThanhToanService : LichSuThanhToanRepository
    {
        public void ThemLichSuThanhToan(LichSuThanhToan lichSuThanhToan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO LichSuThanhToan (ngaythanhtoan, sotienthanhtoan, hinhthucthanhtoan, trangthai,phantramgiamgia, iddatphong, idnhanvien) " +
                    "VALUES (@ngaythanhtoan, @sotienthanhtoan, @hinhthucthanhtoan, @trangthai,@phantramgiamgia, @iddatphong, @idnhanvien);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ngaythanhtoan", lichSuThanhToan.ngaythanhtoan);
                    command.Parameters.AddWithValue("@sotienthanhtoan", lichSuThanhToan.sotienthanhtoan);
                    command.Parameters.AddWithValue("@hinhthucthanhtoan", lichSuThanhToan.hinhthucthanhtoan);
                    command.Parameters.AddWithValue("@trangthai", lichSuThanhToan.trangthai);
                    command.Parameters.AddWithValue("@phantramgiamgia", lichSuThanhToan.phantramgiamgia);
                    command.Parameters.AddWithValue("@iddatphong", lichSuThanhToan.iddatphong);
                    command.Parameters.AddWithValue("@idnhanvien", lichSuThanhToan.idnhanvien);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<LichSuThanhToan> GetAllLichSuThanhToanDescNgayThanhToan()
        {
            List<LichSuThanhToan> lichSuThanhToans = new List<LichSuThanhToan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM LichSuThanhToan ORDER BY ngaythanhtoan DESC";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan();
                            lichSuThanhToan.ngaythanhtoan = reader.GetDateTime("ngaythanhtoan");
                            lichSuThanhToan.sotienthanhtoan = Convert.ToSingle(reader["sotienthanhtoan"]);
                            lichSuThanhToan.hinhthucthanhtoan = reader.GetString("hinhthucthanhtoan");
                            lichSuThanhToan.trangthai = reader.GetString("trangthai");
                            lichSuThanhToan.phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]);
                            lichSuThanhToan.iddatphong = reader.GetInt32("iddatphong");
                            lichSuThanhToan.idnhanvien = reader.GetInt32("idnhanvien");

                            lichSuThanhToans.Add(lichSuThanhToan);
                        }
                    }
                }
            }

            return lichSuThanhToans;
        }
        public List<LichSuThanhToan> GetLichSuThanhToan()
        {
            List<LichSuThanhToan> lichSuThanhToanList = new List<LichSuThanhToan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT MONTH(ngaythanhtoan) as month, SUM(sotienthanhtoan) as total " +
                               "FROM LichSuThanhToan " +
                               "GROUP BY MONTH(ngaythanhtoan) " +
                               "ORDER BY MONTH(ngaythanhtoan)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = Convert.ToInt32(reader["month"]);
                            float total = Convert.ToSingle(reader["total"]);
                            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan
                            {
                                ngaythanhtoan = new DateTime(1, month, 1),
                                sotienthanhtoan = total,
                            };

                            lichSuThanhToanList.Add(lichSuThanhToan);
                        }
                    }
                }
            }

            return lichSuThanhToanList;
        }
    }
}