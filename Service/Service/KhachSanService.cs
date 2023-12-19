using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
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
                                thanhpho = reader["thanhpho"].ToString(),
                                quocgia = reader["quocgia"].ToString(),
                                loaihinh = reader["loaihinh"].ToString(),
                                giayphepkinhdoanh = reader["giayphepkinhdoanh"].ToString(),
                                ngaythanhlap =(DateTime)reader["ngaythanhlap"]
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
                                thanhpho = reader["thanhpho"].ToString(),
                                quocgia = reader["quocgia"].ToString(),
                                loaihinh = reader["loaihinh"].ToString(),
                                giayphepkinhdoanh = reader["giayphepkinhdoanh"].ToString(),
                                ngaythanhlap = (DateTime)reader["ngaythanhlap"]
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
                                thanhpho = reader["thanhpho"].ToString(),
                                quocgia = reader["quocgia"].ToString(),
                                loaihinh = reader["loaihinh"].ToString(),
                                giayphepkinhdoanh = reader["giayphepkinhdoanh"].ToString(),
                                ngaythanhlap = (DateTime)reader["ngaythanhlap"]
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
                string query = "INSERT INTO KhachSan (tenkhachsan,sosao,diachi,thanhpho,quocgia,email,sodienthoai,loaihinh,giayphepkinhdoanh,ngaythanhlap) " +
                    " VALUES (@tenkhachsan,@sosao,@diachi,@thanhpho,@quocgia,@email,@sodienthoai,@loaihinh,@giayphepkinhdoanh,@ngaythanhlap)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenkhachsan", khachSan.tenkhachsan);
                    command.Parameters.AddWithValue("@sosao", khachSan.sosao);
                    command.Parameters.AddWithValue("@diachi", khachSan.diachi);
                    command.Parameters.AddWithValue("@thanhpho", khachSan.thanhpho);
                    command.Parameters.AddWithValue("@quocgia", khachSan.quocgia);
                    command.Parameters.AddWithValue("@email", khachSan.email);
                    command.Parameters.AddWithValue("@sodienthoai", khachSan.sodienthoai);
                    command.Parameters.AddWithValue("@loaihinh", khachSan.loaihinh);
                    command.Parameters.AddWithValue("@giayphepkinhdoanh", khachSan.giayphepkinhdoanh);
                    command.Parameters.AddWithValue("@ngaythanhlap", khachSan.ngaythanhlap);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
