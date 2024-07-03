using ketnoicsdllan1;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BinhLuanService
    {
        public void InsertBinhLuan(BinhLuan binhLuan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = @"INSERT INTO BinhLuan (idkhachhang, noidung, trangthai, idphong)
                           VALUES (@idkhachhang, @noidung, @trangthai, @idphong);";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idkhachhang", binhLuan.idkhachhang);
                command.Parameters.AddWithValue("@noidung", binhLuan.noidung);
                command.Parameters.AddWithValue("@trangthai", binhLuan.trangthai);
                command.Parameters.AddWithValue("@idphong", binhLuan.idphong);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // READ
        public List<BinhLuan> GetAllBinhLuans()
        {
            List<BinhLuan> binhLuans = new List<BinhLuan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan;";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idkhachhang = (int)reader["idkhachhang"],
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"]
                    };
                    binhLuans.Add(binhLuan);
                }
            }
            return binhLuans;
        }


        public List<BinhLuan> GetAllBinhLuansByIDpHONG(int idphong)
        {
            List<BinhLuan> binhLuans = new List<BinhLuan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan WHERE idphong = @idphong;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idphong", idphong);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idkhachhang = (int)reader["idkhachhang"],
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"]
                    };
                    binhLuans.Add(binhLuan);
                }
            }
            return binhLuans;
        }


        // Trong class BinhLuanRepository
        public BinhLuan GetBinhLuanById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan WHERE id = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idkhachhang = (int)reader["idkhachhang"],
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"]
                    };
                    return binhLuan;
                }
                else
                {
                    return null;
                }
            }
        }

        public BinhLuan GetBinhLuanByIdPhong(int idphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan WHERE idphong = @idphong;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idphong", idphong);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idkhachhang = (int)reader["idkhachhang"],
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"]
                    };
                    return binhLuan;
                }
                else
                {
                    return null;
                }
            }
        }

        // UPDATE
        public void UpdateBinhLuan(BinhLuan binhLuan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = @"UPDATE BinhLuan
                           SET idkhachhang = @idkhachhang, 
                               noidung = @noidung, 
                               trangthai = @trangthai, 
                               idphong = @idphong
                           WHERE id = @id;";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idkhachhang", binhLuan.idkhachhang);
                command.Parameters.AddWithValue("@noidung", binhLuan.noidung);
                command.Parameters.AddWithValue("@trangthai", binhLuan.trangthai);
                command.Parameters.AddWithValue("@idphong", binhLuan.idphong);
                command.Parameters.AddWithValue("@id", binhLuan.id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // DELETE
        public void DeleteBinhLuan(int binhLuanId)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "DELETE FROM BinhLuan WHERE id = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", binhLuanId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}