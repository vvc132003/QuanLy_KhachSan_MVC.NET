using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class KhachSanService : KhachSanRepository
    {
        public List<KhachSan> GetAllKhachSan()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<KhachSan> listkhachsan = new List<KhachSan>();
                connection.Open();
                string query = "select * from KhachSan ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachSan khachSan = new KhachSan()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenkhachsan = reader["tenkhachsan"].ToString(),
                                diachi = reader["diachi"].ToString(),
                                sosao = Convert.ToInt32(reader["sosao"]),
                                email = reader["email"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tienich = reader["tienich"].ToString(),
                            };
                            listkhachsan.Add(khachSan);
                        }
                    }
                }
                return listkhachsan;
            }
        }
        public List<KhachSan> GetAllKhachSanByname(string tenkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<KhachSan> listkhachsan = new List<KhachSan>();
                connection.Open();
                string query = "select * from KhachSan where tenkhachsan = @tenkhachsan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenkhachsan", tenkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachSan khachSan = new KhachSan()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenkhachsan = reader["tenkhachsan"].ToString(),
                                diachi = reader["diachi"].ToString(),
                                sosao = Convert.ToInt32(reader["sosao"]),
                                email = reader["email"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tienich = reader["tienich"].ToString(),
                            };
                            listkhachsan.Add(khachSan);
                        }
                    }
                }
                return listkhachsan;
            }
        }

        public KhachSan GetKhachSanById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from KhachSan where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachSan khachSan = new KhachSan
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenkhachsan = reader["tenkhachsan"].ToString(),
                                diachi = reader["diachi"].ToString(),
                                sosao = Convert.ToInt32(reader["sosao"]),
                                email = reader["email"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tienich = reader["tienich"].ToString(),
                            };
                            return khachSan;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemKhachSan(KhachSan khachSan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO KhachSan (tenkhachsan,sosao,diachi,email,sodienthoai,tienich) " +
                    " VALUES (@tenkhachsan,@sosao,@diachi,@email,@sodienthoai,@tienich)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenkhachsan", khachSan.tenkhachsan);
                    command.Parameters.AddWithValue("@sosao", khachSan.sosao);
                    command.Parameters.AddWithValue("@diachi", khachSan.diachi);
                    command.Parameters.AddWithValue("@email", khachSan.email);
                    command.Parameters.AddWithValue("@sodienthoai", khachSan.sodienthoai);
                    command.Parameters.AddWithValue("@tienich", khachSan.tienich);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
