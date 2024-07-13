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
    public class LoaiDichDichVuService
    {
        public void ThemLoaiDichVu(LoaiDichVu loaiDichVu)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO LoaiDichVu (tenloaidichvu, idkhachsan) VALUES (@tenloaidichvu, @idkhachsan)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@tenloaidichvu", loaiDichVu.tenloaidichvu);
                cmd.Parameters.AddWithValue("@idkhachsan", loaiDichVu.idkhachsan);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<LoaiDichVu> LayTatCaLoaiDichVu()
        {
            List<LoaiDichVu> danhSachLoaiDichVu = new List<LoaiDichVu>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT id, tenloaidichvu, idkhachsan FROM LoaiDichVu";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LoaiDichVu loaiDichVu = new LoaiDichVu();
                    loaiDichVu.id = Convert.ToInt32(reader["id"]);
                    loaiDichVu.tenloaidichvu = Convert.ToString(reader["tenloaidichvu"]);
                    loaiDichVu.idkhachsan = Convert.ToInt32(reader["idkhachsan"]);

                    danhSachLoaiDichVu.Add(loaiDichVu);
                }
            }

            return danhSachLoaiDichVu;
        }

        // Hàm cập nhật thông tin một loại dịch vụ
        public void CapNhatLoaiDichVu(LoaiDichVu loaiDichVu)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "UPDATE LoaiDichVu SET tenloaidichvu = @tenloaidichvu, idkhachsan = @idkhachsan WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", loaiDichVu.id);
                cmd.Parameters.AddWithValue("@tenloaidichvu", loaiDichVu.tenloaidichvu);
                cmd.Parameters.AddWithValue("@idkhachsan", loaiDichVu.idkhachsan);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public LoaiDichVu GetLoaiDichVuById(int id)
        {
            LoaiDichVu loaiDichVu = null;

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT id, tenloaidichvu, idkhachsan FROM LoaiDichVu WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    loaiDichVu = new LoaiDichVu();
                    loaiDichVu.id = Convert.ToInt32(reader["id"]);
                    loaiDichVu.tenloaidichvu = Convert.ToString(reader["tenloaidichvu"]);
                    loaiDichVu.idkhachsan = Convert.ToInt32(reader["idkhachsan"]);
                }
            }

            return loaiDichVu;
        }
        // Hàm xóa một loại dịch vụ
        public void XoaLoaiDichVu(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM LoaiDichVu WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
