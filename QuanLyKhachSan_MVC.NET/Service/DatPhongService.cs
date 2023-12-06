﻿using DocumentFormat.OpenXml.Office2010.Excel;
using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class DatPhongService : DatPhongRepository
    {
        public List<DatPhong> GetAllDatPhongByID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<DatPhong> datPhongs = new List<DatPhong>();
                connection.Open();
                string sql = "SELECT * FROM DatPhong left join KhachHang on datphong.idkhachhang = khachhang.id where idphong = @idphong ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idphong", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong()
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idkhachhang = (int)reader["idkhachhang"],
                                hovaten = reader["hovaten"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"],
                                tiendatcoc = Convert.ToSingle(reader["tiendatcoc"]),
                            };
                            datPhongs.Add(datPhong);
                        }
                    }
                }
                return datPhongs;
            }
        }

        public DatPhong GetDatPhongByIDTrangThai(int idphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from DatPhong where idphong = @idphong and trangthai= N'đã đặt' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idphong", idphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idloaidatphong = (int)reader["idloaidatphong"],
                                idkhachhang = (int)reader["idkhachhang"],
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"]
                            };
                            return datPhong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public int ThemDatPhong(DatPhong datPhong)
        {
            int idDatPhongThemVao = 0;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO DatPhong (ngaydat, ngaydukientra, tiendatcoc, trangthai,hinhthucthue, idloaidatphong, idkhachhang, idphong) " +
                               "VALUES (@ngaydat, @ngaydukientra, @tiendatcoc, @trangthai,@hinhthucthue, @idloaidatphong, @idkhachhang, @idphong)" +
                               "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ngaydat", datPhong.ngaydat);
                command.Parameters.AddWithValue("@ngaydukientra", datPhong.ngaydukientra);
                command.Parameters.AddWithValue("@tiendatcoc", datPhong.tiendatcoc);
                command.Parameters.AddWithValue("@trangthai", datPhong.trangthai);
                command.Parameters.AddWithValue("@hinhthucthue", datPhong.hinhthucthue);
                command.Parameters.AddWithValue("@idloaidatphong", datPhong.idloaidatphong);
                command.Parameters.AddWithValue("@idkhachhang", datPhong.idkhachhang);
                command.Parameters.AddWithValue("@idphong", datPhong.idphong);
                idDatPhongThemVao = Convert.ToInt32(command.ExecuteScalar());
            }
            return idDatPhongThemVao;
        }

        public void UpdateDatPhong(DatPhong datPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string updateQuery = "UPDATE DatPhong SET ngaydat = @ngaydat, ngaydukientra = @ngaydukientra, " +
                                   "tiendatcoc = @tiendatcoc, trangthai = @trangthai, hinhthucthue = @hinhthucthue, idloaidatphong=@idloaidatphong," +
                                   " idkhachhang=@idkhachhang, idphong=@idphong " +
                                   "WHERE id = @id";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@ngaydat", datPhong.ngaydat);
                command.Parameters.AddWithValue("@ngaydukientra", datPhong.ngaydukientra);
                command.Parameters.AddWithValue("@tiendatcoc", datPhong.tiendatcoc);
                command.Parameters.AddWithValue("@trangthai", datPhong.trangthai);
                command.Parameters.AddWithValue("@hinhthucthue", datPhong.hinhthucthue);
                command.Parameters.AddWithValue("@idloaidatphong", datPhong.idloaidatphong);
                command.Parameters.AddWithValue("@idkhachhang", datPhong.idkhachhang);
                command.Parameters.AddWithValue("@idphong", datPhong.idphong);
                command.Parameters.AddWithValue("@id", datPhong.id);
                command.ExecuteNonQuery();
            }
        }
    }
}
