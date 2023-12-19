using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class PhongKhachSanService : PhongKhachSanRepository
    {
        public List<PhongKhachSan> GetAllPhongKhachSan()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<PhongKhachSan> phongKhachSans = new List<PhongKhachSan>();
                connection.Open();
                string query = "select * from PhongKhachSan ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PhongKhachSan phongKhachSan = new PhongKhachSan()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                            };
                            phongKhachSans.Add(phongKhachSan);
                        }
                    }
                }
                return phongKhachSans;
            }
        }

        public List<PhongKhachSan> GetAllPhongIDKhachSan(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<PhongKhachSan> phongKhachSans = new List<PhongKhachSan>();
                connection.Open();
                string sql = "SELECT * FROM PhongKhachSan where idkhachsan = @idkhachsan";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PhongKhachSan phongKhachSan = new PhongKhachSan()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                loaiphong = reader["loaiphong"].ToString(),
                                tinhtrangphong = reader["tinhtrangphong"].ToString(),
                                giatientheogio = Convert.ToSingle(reader["giatientheogio"]),
                                giatientheongay = Convert.ToSingle(reader["giatientheongay"]),
                                idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                            };
                            phongKhachSans.Add(phongKhachSan);
                        }
                    }
                }
                return phongKhachSans;
            }
        }

        public void ThemPhongKhachSan(PhongKhachSan phongKhachSan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO PhongKhachSan (loaiphong,tinhtrangphong,giatientheogio,giatientheongay,idkhachsan) " +
                    " VALUES (@loaiphong,@tinhtrangphong,@giatientheogio,@giatientheongay,@idkhachsan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tinhtrangphong", phongKhachSan.tinhtrangphong);
                    command.Parameters.AddWithValue("@loaiphong", phongKhachSan.loaiphong);
                    command.Parameters.AddWithValue("@giatientheogio", phongKhachSan.giatientheogio);
                    command.Parameters.AddWithValue("@giatientheongay", phongKhachSan.giatientheongay);
                    command.Parameters.AddWithValue("@idkhachsan", phongKhachSan.idkhachsan);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
