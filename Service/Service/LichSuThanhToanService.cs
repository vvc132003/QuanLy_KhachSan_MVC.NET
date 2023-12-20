﻿using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
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
    }
}