﻿using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class ThoiGianService : ThoiGianRepository
    {
        public void CapNhatThoiGian(ThoiGian thoiGian)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ThoiGian SET thoigiannhanphong=@thoigiannhanphong, thoigianra = @thoigianra, mota = @mota WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@thoigiannhanphong", thoiGian.thoigiannhanphong);
                    command.Parameters.AddWithValue("@thoigianra", thoiGian.thoigianra);
                    command.Parameters.AddWithValue("@mota", thoiGian.mota);
                    command.Parameters.AddWithValue("@id", thoiGian.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ThoiGian> GetAllThoiGian()
        {
            List<ThoiGian> danhSachThoiGian = new List<ThoiGian>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM ThoiGian";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian
                    {
                        id = Convert.ToInt32(reader["id"]),
                        thoigiannhanphong = Convert.ToDateTime(reader["thoigiannhanphong"]),
                        thoigianra = Convert.ToDateTime(reader["thoigianra"]),
                        mota = Convert.ToString(reader["mota"])
                    };
                    danhSachThoiGian.Add(thoiGian);
                }
                reader.Close();
            }

            return danhSachThoiGian;
        }

        public ThoiGian GetThoiGian(DateTime thoigiannhanphong, int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThoiGian WHERE CONVERT(date, thoigiannhanphong) = CONVERT(date, @thoigiannhanphong) and idkhachsan=@idkhachsan ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@thoigiannhanphong", thoigiannhanphong);
                command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        thoigiannhanphong = Convert.ToDateTime(reader["thoigiannhanphong"]),
                        thoigianra = Convert.ToDateTime(reader["thoigianra"]),
                        mota = Convert.ToString(reader["mota"]),
                    };
                    return thoiGian;
                }
                else
                {
                    return null;
                }
            }
        }
        public ThoiGian GetThoiGianById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThoiGian WHERE id = @id ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        thoigiannhanphong = Convert.ToDateTime(reader["thoigiannhanphong"]),
                        thoigianra = Convert.ToDateTime(reader["thoigianra"]),
                        mota = Convert.ToString(reader["mota"]),
                    };
                    return thoiGian;
                }
                else
                {
                    return null;
                }
            }
        }

        public void ThemThoiGian(ThoiGian thoiGian)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThoiGian (thoigiannhanphong, thoigianra, mota,idkhachsan) " +
                    "VALUES (@thoigiannhanphong, @thoigianra, @mota,@idkhachsan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@thoigiannhanphong", thoiGian.thoigiannhanphong);
                    command.Parameters.AddWithValue("@thoigianra", thoiGian.thoigianra);
                    command.Parameters.AddWithValue("@mota", thoiGian.mota);
                    command.Parameters.AddWithValue("@idkhachsan", thoiGian.idkhachsan);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
