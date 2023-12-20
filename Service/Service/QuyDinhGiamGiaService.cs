using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace Service
{
    public class QuyDinhGiamGiaService : QuyDinhGiamGiaRepository
    {
        public void CapNhatQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE QuyDinhGiamGia SET soluongdatphongtoithieu = @soluongdatphongtoithieu,tongtientoithieu=@tongtientoithieu," +
                    " thoigiandatphong=@thoigiandatphong,  phantramgiamgia = @phantramgiamgia, ngaythemquydinh=@ngaythemquydinh ,idkhachsan@idkhachsan" +
                    " WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@soluongdatphongtoithieu", quyDinhGiamGia.soluongdatphongtoithieu);
                command.Parameters.AddWithValue("@tongtientoithieu", quyDinhGiamGia.tongtientoithieu);
                command.Parameters.AddWithValue("@thoigiandatphong", quyDinhGiamGia.thoigiandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", quyDinhGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemquydinh", quyDinhGiamGia.ngaythemquydinh);
                command.Parameters.AddWithValue("@idkhachsan", quyDinhGiamGia.idkhachsan);
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
                        soluongdatphongtoithieu = Convert.ToInt32(reader["soluongdatphongtoithieu"]),
                        tongtientoithieu = Convert.ToInt32(reader["tongtientoithieu"]),
                        thoigiandatphong = Convert.ToInt32(reader["thoigiandatphong"]),
                        phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                        ngaythemquydinh = Convert.ToDateTime(reader["ngaythemquydinh"]),
                        idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                    };
                    danhSachQuyDinhGiamGia.Add(quyDinhGiamGia);
                }
            }
            return danhSachQuyDinhGiamGia;
        }

        public List<QuyDinhGiamGia> GetQuyDinhGia(int idkhachsan)
        {
            List<QuyDinhGiamGia> danhSachQuyDinhGiamGia = new List<QuyDinhGiamGia>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM QuyDinhGiamGia WHERE idkhachsan = @idkhachsan";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuyDinhGiamGia quyDinhGiamGia = new QuyDinhGiamGia()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        soluongdatphongtoithieu = Convert.ToInt32(reader["soluongdatphongtoithieu"]),
                        tongtientoithieu = Convert.ToInt32(reader["tongtientoithieu"]),
                        thoigiandatphong = Convert.ToInt32(reader["thoigiandatphong"]),
                        phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                        ngaythemquydinh = Convert.ToDateTime(reader["ngaythemquydinh"]),
                        idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
                    };
                    danhSachQuyDinhGiamGia.Add(quyDinhGiamGia);
                }
            }
            return danhSachQuyDinhGiamGia;
        }
        public QuyDinhGiamGia GetQuyDinhGiasolandatphong(float soluongdatphongtoithieu)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM QuyDinhGiamGia WHERE soluongdatphongtoithieu = @soluongdatphongtoithieu";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@soluongdatphongtoithieu", soluongdatphongtoithieu);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    QuyDinhGiamGia quyDinhGiamGia = new QuyDinhGiamGia()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        soluongdatphongtoithieu = Convert.ToInt32(reader["soluongdatphongtoithieu"]),
                        tongtientoithieu = Convert.ToInt32(reader["tongtientoithieu"]),
                        thoigiandatphong = Convert.ToInt32(reader["thoigiandatphong"]),
                        phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
                        ngaythemquydinh = Convert.ToDateTime(reader["ngaythemquydinh"]),
                        idkhachsan = Convert.ToInt32(reader["idkhachsan"]),
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
                string query = "INSERT INTO QuyDinhGiamGia (soluongdatphongtoithieu,tongtientoithieu,thoigiandatphong, phantramgiamgia,ngaythemquydinh,idkhachsan) " +
                    "VALUES (@solandatphong,@tongtientoithieu,@thoigiandatphong, @phantramgiamgia,@ngaythemquydinh,@idkhachsan)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@soluongdatphongtoithieu", quyDinhGiamGia.soluongdatphongtoithieu);
                command.Parameters.AddWithValue("@tongtientoithieu", quyDinhGiamGia.tongtientoithieu);
                command.Parameters.AddWithValue("@thoigiandatphong", quyDinhGiamGia.thoigiandatphong);
                command.Parameters.AddWithValue("@phantramgiamgia", quyDinhGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@ngaythemquydinh", quyDinhGiamGia.ngaythemquydinh);
                command.Parameters.AddWithValue("@idkhachsan", quyDinhGiamGia.idkhachsan);
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