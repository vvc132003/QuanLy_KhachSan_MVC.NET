using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class QuyDinhGiamGiaService : QuyDinhGiamGiaRepository
    {
        public void CapNhatQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE QuyDinhGiamGia SET solandatphong = @solandatphong, phantramgiamgia = @phantramgiamgia, ngaythemquydinh=@ngaythemquydinh " +
                    " WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@solandatphong", quyDinhGiamGia.solandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", quyDinhGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemquydinh", quyDinhGiamGia.ngaythemquydinh);
                command.Parameters.AddWithValue("@id", quyDinhGiamGia.id);
                command.ExecuteNonQuery();
            }
        }

        public List<QuyDinhGiamGia> GetAllQuyDinhGia()
        {
            List<QuyDinhGiamGia> danhSachQuyDinhGiamGia = new List<QuyDinhGiamGia>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM QuyDinhGiamGia";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuyDinhGiamGia quyDinhGiamGia = new QuyDinhGiamGia()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        solandatphong = Convert.ToInt32(reader["solandatphong"]),
                        phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                        ngaythemquydinh = Convert.ToDateTime(reader["ngaythemquydinh"]),
                    };
                    danhSachQuyDinhGiamGia.Add(quyDinhGiamGia);
                }
            }
            return danhSachQuyDinhGiamGia;
        }

        public QuyDinhGiamGia GetQuyDinhGia(float solandatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM QuyDinhGiamGia WHERE solandatphong = @solandatphong";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@solandatphong", solandatphong);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    QuyDinhGiamGia quyDinhGiamGia = new QuyDinhGiamGia()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        solandatphong = Convert.ToInt32(reader["solandatphong"]),
                        phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                        ngaythemquydinh = Convert.ToDateTime(reader["ngaythemquydinh"]),
                    };
                    return quyDinhGiamGia;
                }
                else
                {
                    return null;
                }
            }
        }

        public void ThemQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO QuyDinhGiamGia (solandatphong, phantramgiamgia,ngaythemquydinh) VALUES (@solandatphong, @phantramgiamgia,@ngaythemquydinh)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@solandatphong", quyDinhGiamGia.solandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", quyDinhGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemquydinh", quyDinhGiamGia.ngaythemquydinh);
                command.ExecuteNonQuery();
            }
        }

        public void XoaQuyDinhGiamGia(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM QuyDinhGiamGia WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
