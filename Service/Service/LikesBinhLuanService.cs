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
    public class LikesBinhLuanService
    {
        public void InsertLike(LikesBinhLuan like)
        {
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "INSERT INTO LikesBinhLuan (idbinhluan, idkhachhang, thoigianlike, thich, khongthich) " +
                           "VALUES (@idbinhluan, @idkhachhang, @thoigianlike, @thich, @khongthich)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idbinhluan", like.idbinhluan);
            command.Parameters.AddWithValue("@idkhachhang", like.idkhachhang);
            command.Parameters.AddWithValue("@thoigianlike", like.thoigianlike);
            command.Parameters.AddWithValue("@thich", like.thich);
            command.Parameters.AddWithValue("@khongthich", like.khongthich);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public List<LikesBinhLuan> GetAllLikes()
        {
            SqlConnection connection = DBUtils.GetDBConnection();
            List<LikesBinhLuan> likesList = new List<LikesBinhLuan>();
            string query = "SELECT * FROM LikesBinhLuan";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                LikesBinhLuan like = new LikesBinhLuan();
                like.id = Convert.ToInt32(reader["id"]);
                like.idbinhluan = Convert.ToInt32(reader["idbinhluan"]);
                like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                like.thich = Convert.ToInt32(reader["thich"]);
                like.khongthich = Convert.ToInt32(reader["khongthich"]);
                likesList.Add(like);
            }
            reader.Close();
            connection.Close();
            return likesList;
        }
        public void UpdateLike(LikesBinhLuan like)
        {
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "UPDATE LikesBinhLuan " +
                           "SET idbinhluan = @idbinhluan, idkhachhang = @idkhachhang, thoigianlike = @thoigianlike, thich=@thich, khongthich=@khongthich " +
                           "WHERE id = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idbinhluan", like.idbinhluan);
            command.Parameters.AddWithValue("@idkhachhang", like.idkhachhang);
            command.Parameters.AddWithValue("@thoigianlike", like.thoigianlike);
            command.Parameters.AddWithValue("@thich", like.thich);
            command.Parameters.AddWithValue("@khongthich", like.khongthich);

            command.Parameters.AddWithValue("@id", like.id);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteLike(int likeId)
        {
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "DELETE FROM LikesBinhLuan WHERE id = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", likeId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public LikesBinhLuan GetLikesBinhLuanById(int likeId)
        {
            LikesBinhLuan like = null;
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "SELECT * FROM LikesBinhLuan WHERE id = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", likeId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                like = new LikesBinhLuan();
                like.id = Convert.ToInt32(reader["id"]);
                like.idbinhluan = Convert.ToInt32(reader["idbinhluan"]);
                like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                like.thich = Convert.ToInt32(reader["thich"]);
                like.khongthich = Convert.ToInt32(reader["khongthich"]);
            }
            reader.Close();
            connection.Close();
            return like;
        }
        public LikesBinhLuan GetLikesBinhLuanByidbinhluansupdate(int idbinhluan)
        {
            LikesBinhLuan like = null;
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "SELECT * FROM LikesBinhLuan WHERE idbinhluan = @idbinhluan";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idbinhluan", idbinhluan);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                like = new LikesBinhLuan();
                like.id = Convert.ToInt32(reader["id"]);
                like.idbinhluan = Convert.ToInt32(reader["idbinhluan"]);
                like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                like.thich = Convert.ToInt32(reader["thich"]);
                like.khongthich = Convert.ToInt32(reader["khongthich"]);
            }
            reader.Close();
            connection.Close();
            return like;
        }
        public LikesBinhLuan CheckLikesBinhLuanIDbinhluanandIdKhachHang(int idbinhluan, int idkhachhang)
        {
            LikesBinhLuan like = null;
            SqlConnection connection = DBUtils.GetDBConnection();
            string query = "SELECT * FROM LikesBinhLuan WHERE idbinhluan = @idbinhluan and idkhachhang = @idkhachhang";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idbinhluan", idbinhluan);
            command.Parameters.AddWithValue("@idkhachhang", idkhachhang);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                like = new LikesBinhLuan();
                like.id = Convert.ToInt32(reader["id"]);
                like.idbinhluan = Convert.ToInt32(reader["idbinhluan"]);
                like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                like.thich = Convert.ToInt32(reader["thich"]);
                like.khongthich = Convert.ToInt32(reader["khongthich"]);
            }
            reader.Close();
            connection.Close();
            return like;
        }

        public List<LikesBinhLuan> GetLikesBinhLuanByidbinhluan(int idbinhluan)
        {
            SqlConnection connection = DBUtils.GetDBConnection();
            List<LikesBinhLuan> likesList = new List<LikesBinhLuan>();
            string query = "SELECT * FROM LikesBinhLuan WHERE idbinhluan = @idbinhluan";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idbinhluan", idbinhluan);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                LikesBinhLuan like = new LikesBinhLuan();
                like.id = Convert.ToInt32(reader["id"]);
                like.idbinhluan = Convert.ToInt32(reader["idbinhluan"]);
                like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                like.thich = Convert.ToInt32(reader["thich"]);
                like.khongthich = Convert.ToInt32(reader["khongthich"]);
                likesList.Add(like);
            }
            reader.Close();
            connection.Close();
            return likesList;
        }
    }
}
