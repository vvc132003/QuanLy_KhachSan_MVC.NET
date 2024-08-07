﻿using DocumentFormat.OpenXml.Office2010.Excel;
using ketnoicsdllan1;
using NPOI.SS.Formula.Functions;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class ThueSanPhamService : ThueSanPhamRepository
    {
        public List<ThueSanPham> GetAllThueSanPham()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),
                            };
                            thueSanPhams.Add(thueSanPham);
                        }
                    }
                }
                return thueSanPhams;
            }
        }

        public List<ThueSanPham> GetThueSanPhamByDate(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham WHERE ngayThue >= @startDate AND ngayThue <= @endDate";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),

                            };
                            thueSanPhams.Add(thueSanPham);
                        }
                    }
                }
                return thueSanPhams;
            }
        }

        public List<ThueSanPham> GetThueSanPhamByDatebyidddatphong(DateTime startDate, DateTime endDate, int iddatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham WHERE ngayThue >= @startDate AND ngayThue <= @endDate and iddatphong = @iddatphong";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@iddatphong", iddatphong);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),

                            };
                            thueSanPhams.Add(thueSanPham);
                        }
                    }
                }
                return thueSanPhams;
            }
        }

        public List<ThueSanPham> GetThueSanPhamByIDdatphong(int iddatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham WHERE iddatphong = @iddatphong";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@iddatphong", iddatphong);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),

                            };
                            thueSanPhams.Add(thueSanPham);
                        }
                    }
                }
                return thueSanPhams;
            }
        }

        public ThueSanPham GetThueSanPhamByDatPhongAndSanPham(int iddatphong, int idsanpham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham WHERE iddatphong = @iddatphong AND idsanpham = @idsanpham";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@iddatphong", iddatphong);
                    command.Parameters.AddWithValue("@idsanpham", idsanpham);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                iddatphong = (int)reader["iddatphong"],
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),


                            };
                            return thueSanPham;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void CapNhatThueSanPham(ThueSanPham thueSanPham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "UPDATE ThueSanPham SET soluong = @soluong, thanhtien = @thanhtien, idsanpham = @idsanpham, idnhanvien = @idnhanvien, iddatphong = @iddatphong, ghichu = @ghichu " +
                             "WHERE id = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@soluong", thueSanPham.soluong);
                    command.Parameters.AddWithValue("@thanhtien", thueSanPham.thanhtien);
                    command.Parameters.AddWithValue("@idsanpham", thueSanPham.idsanpham);
                    command.Parameters.AddWithValue("@idnhanvien", thueSanPham.idnhanvien);
                    command.Parameters.AddWithValue("@iddatphong", thueSanPham.iddatphong);
                    command.Parameters.AddWithValue("@ghichu", thueSanPham.ghichu);

                    command.Parameters.AddWithValue("@id", thueSanPham.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ThueSanPham> GetAllThueSanPhamID(int iddatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham left join SanPham on ThueSanPham.idsanpham = SanPham.id where iddatphong = @iddatphong ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@iddatphong", iddatphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        /*                        float tongtien = 0;
                        */
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                tensanpham = reader["tensanpham"].ToString(),
                                image = reader["image"].ToString(),
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),

                            };
                            thueSanPhams.Add(thueSanPham);
                        }
                    }
                }
                return thueSanPhams;
            }
        }


        public List<ThueSanPham> GetAllThueSanPhamIDSanPham(int idsanpham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham where idsanpham = @idsanpham ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idsanpham", idsanpham);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                iddatphong = (int)reader["iddatphong"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),


                            };
                            thueSanPhams.Add(thueSanPham);

                        }
                    }
                }
                return thueSanPhams;
            }
        }

        public ThueSanPham GetThueSanPhamBYID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM ThueSanPham where id = @id ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham()
                            {
                                id = (int)reader["id"],
                                soluong = (int)reader["soluong"],
                                idsanpham = (int)reader["idsanpham"],
                                idnhanvien = (int)reader["idnhanvien"],
                                thanhtien = Convert.ToSingle(reader["thanhtien"]),
                                iddatphong = (int)reader["iddatphong"],
                                ngaythue = (DateTime)reader["ngayThue"],
                                ghichu = reader["ghichu"].ToString(),


                            };
                            return thueSanPham;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThueSanPham(ThueSanPham thueSanPham)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO ThueSanPham (soluong, thanhtien, idsanpham, idnhanvien, iddatphong, ghichu) " +
                                     "VALUES (@soluong, @thanhtien, @idsanpham, @idnhanvien, @iddatphong, @ghichu)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@soluong", thueSanPham.soluong);
                    command.Parameters.AddWithValue("@thanhtien", thueSanPham.thanhtien);
                    command.Parameters.AddWithValue("@idsanpham", thueSanPham.idsanpham);
                    command.Parameters.AddWithValue("@idnhanvien", thueSanPham.idnhanvien);
                    command.Parameters.AddWithValue("@iddatphong", thueSanPham.iddatphong);
                    if (thueSanPham.ghichu != null)
                    {
                        command.Parameters.AddWithValue("@ghichu", thueSanPham.ghichu);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ghichu", "...");
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaThueSanPham(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "DELETE FROM ThueSanPham WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
