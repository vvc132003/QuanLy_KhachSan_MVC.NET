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
                string sql = @"INSERT INTO BinhLuan (idnguoithamgia, loainguoithamgia, noidung, trangthai, idphong, parent_comment_id, thich, khongthich)
                       VALUES (@idnguoithamgia, @loainguoithamgia, @noidung, @trangthai, @idphong, @parent_comment_id, @thich, @khongthich);";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idnguoithamgia", binhLuan.idnguoithamgia);
                command.Parameters.AddWithValue("@loainguoithamgia", binhLuan.loainguoithamgia);
                command.Parameters.AddWithValue("@noidung", binhLuan.noidung);
                command.Parameters.AddWithValue("@trangthai", binhLuan.trangthai);
                command.Parameters.AddWithValue("@idphong", binhLuan.idphong);
                command.Parameters.AddWithValue("@parent_comment_id", binhLuan.parent_comment_id);
                command.Parameters.AddWithValue("@thich", binhLuan.thich);
                command.Parameters.AddWithValue("@khongthich", binhLuan.khongthich);

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
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0,
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),

                    };
                    binhLuans.Add(binhLuan);
                }
            }
            return binhLuans;
        }

        public List<BinhLuan> GetBinhLuanByPhong(int idphong)
        {
            List<BinhLuan> binhLuanList = new List<BinhLuan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = @"
                    SELECT bl.id, bl.idnguoithamgia, bl.loainguoithamgia, bl.noidung, 
                       bl.thoigianbinhluan, bl.trangthai, bl.idphong, bl.parent_comment_id, bl.thich, bl.khongthich, 
                       COALESCE(nv.hovaten, kh.hovaten) AS hovaten
                        FROM BinhLuan bl
                        LEFT JOIN NhanVien nv ON bl.loainguoithamgia = 'nhanVien' AND bl.idnguoithamgia = nv.id
                        LEFT JOIN KhachHang kh ON bl.loainguoithamgia = 'khachHang' AND bl.idnguoithamgia = kh.id
                        WHERE bl.idphong = @idphong
                        ORDER BY bl.thoigianbinhluan";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idphong", idphong);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BinhLuan binhLuan = new BinhLuan();
                        binhLuan.id = Convert.ToInt32(reader["id"]);
                        binhLuan.idnguoithamgia = Convert.ToInt32(reader["idnguoithamgia"]);
                        binhLuan.loainguoithamgia = reader["loainguoithamgia"].ToString();
                        binhLuan.noidung = reader["noidung"].ToString();
                        binhLuan.thoigianbinhluan = Convert.ToDateTime(reader["thoigianbinhluan"]);
                        binhLuan.trangthai = reader["trangthai"].ToString();
                        binhLuan.idphong = Convert.ToInt32(reader["idphong"]);
                        binhLuan.thich = Convert.ToInt32(reader["thich"]);
                        binhLuan.khongthich = Convert.ToInt32(reader["khongthich"]);

                        if (reader["parent_comment_id"] != DBNull.Value)
                        {
                            binhLuan.parent_comment_id = Convert.ToInt32(reader["parent_comment_id"]);
                        }
                        binhLuan.hovaten = reader["hovaten"].ToString();

                        binhLuanList.Add(binhLuan);
                    }
                }
            }

            return binhLuanList;
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
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0
                    };
                    binhLuans.Add(binhLuan);
                }
            }
            return binhLuans;
        }
        public List<BinhLuan> GetAllBinhLuansByIparent_comment_id(int parent_comment_id)
        {
            List<BinhLuan> binhLuans = new List<BinhLuan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan WHERE parent_comment_id = @parent_comment_id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@parent_comment_id", parent_comment_id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0
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
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0
                    };
                    return binhLuan;
                }
                else
                {
                    return null;
                }
            }
        }

        public BinhLuan GetBinhLuanByIdparent_comment_id(int parent_comment_id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM BinhLuan WHERE parent_comment_id = @parent_comment_id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@parent_comment_id", parent_comment_id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    BinhLuan binhLuan = new BinhLuan
                    {
                        id = (int)reader["id"],
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0
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
                        idnguoithamgia = (int)reader["idnguoithamgia"],
                        loainguoithamgia = reader["loainguoithamgia"].ToString(),
                        noidung = reader["noidung"].ToString(),
                        thoigianbinhluan = (DateTime)reader["thoigianbinhluan"],
                        trangthai = reader["trangthai"].ToString(),
                        idphong = (int)reader["idphong"],
                        thich = Convert.ToInt32(reader["thich"]),
                        khongthich = Convert.ToInt32(reader["khongthich"]),
                        parent_comment_id = reader["parent_comment_id"] != DBNull.Value ? (int)reader["parent_comment_id"] : 0
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
                       SET idnguoithamgia = @idnguoithamgia,
                           loainguoithamgia = @loainguoithamgia,
                           noidung = @noidung,
                           trangthai = @trangthai,
                           thich=@thich,
                           khongthich=@khongthich,
                           idphong = @idphong,
                           parent_comment_id = @parent_comment_id
                       WHERE id = @id;";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idnguoithamgia", binhLuan.idnguoithamgia);
                command.Parameters.AddWithValue("@loainguoithamgia", binhLuan.loainguoithamgia);
                command.Parameters.AddWithValue("@noidung", binhLuan.noidung);
                command.Parameters.AddWithValue("@trangthai", binhLuan.trangthai);
                command.Parameters.AddWithValue("@idphong", binhLuan.idphong);
                command.Parameters.AddWithValue("@parent_comment_id", binhLuan.parent_comment_id);
                command.Parameters.AddWithValue("@thich", binhLuan.thich);
                command.Parameters.AddWithValue("@khongthich", binhLuan.khongthich);

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