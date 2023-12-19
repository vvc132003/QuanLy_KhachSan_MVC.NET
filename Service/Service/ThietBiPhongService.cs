using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class ThietBiPhongService : ThietBiPhongRepository
    {

        public void CapNhatThietBiPhong(ThietBiPhong thietBiPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ThietBiPhong SET ngayduavao = @ngayduavao, soluongduavao = @soluongduavao, idphong = @idphong, idthietbi = @idthietbi WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngayduavao", thietBiPhong.ngayduavao);
                    command.Parameters.AddWithValue("@soluongduavao", thietBiPhong.soluongduavao);
                    command.Parameters.AddWithValue("@idphong", thietBiPhong.idphong);
                    command.Parameters.AddWithValue("@idthietbi", thietBiPhong.idthietbi);
                    command.Parameters.AddWithValue("@id", thietBiPhong.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ThietBiPhong> GetALLThietBiPhong()
        {
            List<ThietBiPhong> thietBiPhongs = new List<ThietBiPhong>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM ThietBiPhong left join ThietBi on ThietBiPhong.idthietbi = ThietBi.id ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ThietBiPhong thietBiPhong = new ThietBiPhong()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            ngayduavao = Convert.ToDateTime(reader["ngayduavao"]),
                            soluongduavao = Convert.ToInt32(reader["soluongduavao"]),
                            idphong = Convert.ToInt32(reader["idphong"]),
                            idthietbi = Convert.ToInt32(reader["idthietbi"]),
                            giathietbi = Convert.ToSingle(reader["giathietbi"]),
                            tenthietbi = reader["tenthietbi"].ToString(),
                        };
                        thietBiPhongs.Add(thietBiPhong);
                    }
                }
            }
            return thietBiPhongs;
        }
        public int SumThietBiPhong()
        {
            int totalCount = 0;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT count(ThietBiPhong.id) FROM ThietBiPhong left join ThietBi on ThietBiPhong.idthietbi = ThietBi.id ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        totalCount = Convert.ToInt32(result);
                    }
                }
            }
            return totalCount;
        }

        public ThietBiPhong GetALLThietBiPhongBYID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThietBiPhong WHERE id = @id ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ThietBiPhong thietBiPhong = new ThietBiPhong()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                ngayduavao = Convert.ToDateTime(reader["ngayduavao"]),
                                soluongduavao = Convert.ToInt32(reader["soluongduavao"]),
                                idphong = Convert.ToInt32(reader["idphong"]),
                                idthietbi = Convert.ToInt32(reader["idthietbi"]),
                            };
                            return thietBiPhong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public List<ThietBiPhong> GetALLThietBiPhongbyIDPhong(int idphong)
        {
            List<ThietBiPhong> thietBiPhongs = new List<ThietBiPhong>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThietBiPhong WHERE idphong = @idphong ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idphong", idphong);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ThietBiPhong thietBiPhong = new ThietBiPhong()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            ngayduavao = Convert.ToDateTime(reader["ngayduavao"]),
                            soluongduavao = Convert.ToInt32(reader["soluongduavao"]),
                            idphong = Convert.ToInt32(reader["idphong"]),
                            idthietbi = Convert.ToInt32(reader["idthietbi"]),
                        };
                        thietBiPhongs.Add(thietBiPhong);
                    }
                }
            }
            return thietBiPhongs;
        }

        public void ThemThietBiPhong(ThietBiPhong thietBiPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThietBiPhong (ngayduavao, soluongduavao, idphong, idthietbi) VALUES (@ngayduavao, @soluongduavao, @idphong, @idthietbi)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngayduavao", thietBiPhong.ngayduavao);
                    command.Parameters.AddWithValue("@soluongduavao", thietBiPhong.soluongduavao);
                    command.Parameters.AddWithValue("@idphong", thietBiPhong.idphong);
                    command.Parameters.AddWithValue("@idthietbi", thietBiPhong.idthietbi);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaThietBiPhong(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM ThietBiPhong WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
