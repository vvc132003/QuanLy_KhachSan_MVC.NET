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
    public class LikesService
    {
        public bool CheckIfLiked(int idKhachHang, int idPhong)
        {
            bool liked = false;

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Likes WHERE idphong = @IdPhong AND idkhachhang = @IdKhachHang";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPhong", idPhong);
                    command.Parameters.AddWithValue("@IdKhachHang", idKhachHang);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        liked = true;
                    }
                }
            }

            return liked;
        }

        public int CountLikesByIdPhong(int idPhong)
        {
            int countLikes = 0;

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT count(idkhachhang) AS TotalLikes FROM Likes WHERE idphong = @IdPhong";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPhong", idPhong);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        countLikes = Convert.ToInt32(result);
                    }
                }
            }

            return countLikes;
        }

        public void CapNhatLike(Likes like)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE Likes SET idphong = @IdPhong, idkhachhang = @IdKhachHang, thoigianlike = @ThoiGianLike, icons = @Icons WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPhong", like.idphong);
                    command.Parameters.AddWithValue("@IdKhachHang", like.idkhachhang);
                    command.Parameters.AddWithValue("@ThoiGianLike", like.thoigianlike);
                    command.Parameters.AddWithValue("@Icons", like.icons);
                    command.Parameters.AddWithValue("@Id", like.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Likes> GetAllLikes()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Likes> likesList = new List<Likes>();
                connection.Open();
                string query = "SELECT id, idphong, idkhachhang, thoigianlike, icons FROM Likes";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Likes like = new Likes();
                            like.id = Convert.ToInt32(reader["id"]);
                            like.idphong = Convert.ToInt32(reader["idphong"]);
                            like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                            like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                            like.icons = reader["icons"].ToString();
                            likesList.Add(like);
                        }
                    }
                }
                return likesList;
            }
        }

        public Likes GetLikeById(int likeId)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT id, idphong, idkhachhang, thoigianlike, icons FROM Likes WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", likeId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Likes like = new Likes();
                            like.id = Convert.ToInt32(reader["id"]);
                            like.idphong = Convert.ToInt32(reader["idphong"]);
                            like.idkhachhang = Convert.ToInt32(reader["idkhachhang"]);
                            like.thoigianlike = Convert.ToDateTime(reader["thoigianlike"]);
                            like.icons = reader["icons"].ToString();

                            return like;
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertLike(Likes like)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO Likes (idphong, idkhachhang, icons) VALUES (@IdPhong, @IdKhachHang, @Icons)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPhong", like.idphong);
                    command.Parameters.AddWithValue("@IdKhachHang", like.idkhachhang);
                    command.Parameters.AddWithValue("@Icons", like.icons);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLike(int likeId)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM Likes WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", likeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
