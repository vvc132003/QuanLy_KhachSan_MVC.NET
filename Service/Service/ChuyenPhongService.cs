﻿using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
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
                    if (chuyenPhong.lydo != null)
                    {
                        command.Parameters.AddWithValue("@lydo", chuyenPhong.lydo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@lydo", "...");
                    }
                    command.Parameters.AddWithValue("@idkhachhang", chuyenPhong.idkhachhang);
                    command.Parameters.AddWithValue("@idnhanvien", chuyenPhong.idnhanvien);
                    command.Parameters.AddWithValue("@idphongcu", chuyenPhong.idphongcu);
                    command.Parameters.AddWithValue("@idphongmoi", chuyenPhong.idphongmoi);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<ChuyenPhong> GetAllChuyenPhongDescNgayChuyen()
        {
            List<ChuyenPhong> chuyenPhongs = new List<ChuyenPhong>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM ChuyenPhong ORDER BY ngaychuyen DESC";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChuyenPhong chuyenPhong = new ChuyenPhong();
                            chuyenPhong.ngaychuyen = reader.GetDateTime("ngaychuyen");
                            chuyenPhong.lydo = reader.GetString("lydo");
                            chuyenPhong.idkhachhang = reader.GetInt32("idkhachhang");
                            chuyenPhong.idnhanvien = reader.GetInt32("idnhanvien");
                            chuyenPhong.idphongcu = reader.GetInt32("idphongcu");
                            chuyenPhong.idphongmoi = reader.GetInt32("idphongmoi");
                            chuyenPhongs.Add(chuyenPhong);
                        }
                    }
                }
            }

            return chuyenPhongs;
        }
        public List<ChuyenPhong> GetAllChuyenPhong()
        {
            List<ChuyenPhong> chuyenPhongs = new List<ChuyenPhong>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM ChuyenPhong ";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChuyenPhong chuyenPhong = new ChuyenPhong();
                            chuyenPhong.ngaychuyen = reader.GetDateTime("ngaychuyen");
                            chuyenPhong.lydo = reader.GetString("lydo");
                            chuyenPhong.idkhachhang = reader.GetInt32("idkhachhang");
                            chuyenPhong.idnhanvien = reader.GetInt32("idnhanvien");
                            chuyenPhong.idphongcu = reader.GetInt32("idphongcu");
                            chuyenPhong.idphongmoi = reader.GetInt32("idphongmoi");
                            chuyenPhongs.Add(chuyenPhong);
                        }
                    }
                }
            }

            return chuyenPhongs;
        }
    }
}
