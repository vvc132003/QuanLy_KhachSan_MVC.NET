using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class TangService : TangRepository
    {
        public void CapNhatTang(Tang tang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                /*                string query = "UPDATE Tang SET tentang = @tentang WHERE id = @id";
                */
                using (SqlCommand command = new SqlCommand("UpdateTang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tentang", tang.tentang);
                    command.Parameters.AddWithValue("@id", tang.id);
                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Tang> GetAllTang(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<Tang> tangs = new List<Tang>();
                connection.Open();
                string query = "select * from Tang where idkhachsan=@idkhachsan ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tang tang = new Tang()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tentang = reader["tentang"].ToString(),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                            };
                            tangs.Add(tang);
                        }
                    }
                }
                return tangs;
            }
        }
        public Tang GetTangidkhachsan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from Tang where idkhachsan = @idkhachsan";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Tang tang = new Tang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tentang = reader["tentang"].ToString(),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                            };
                            return tang;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public Tang GetTangID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from Tang where id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Tang tang = new Tang
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tentang = reader["tentang"].ToString(),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                            };
                            return tang;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemTang(Tang tang)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO Tang (tentang,idkhachsan) VALUES (@tentang,@idkhachsan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tentang", tang.tentang);
                    command.Parameters.AddWithValue("@idkhachsan", tang.idkhachsan);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaTang(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                /*                string query = "DELETE FROM Tang WHERE id = @id";
                */
                using (SqlCommand command = new SqlCommand("DeleteTang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}