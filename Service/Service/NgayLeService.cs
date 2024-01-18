using DocumentFormat.OpenXml.Office2010.Excel;
using ketnoicsdllan1;
using Model.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class NgayLeService : NgayLeRepository
    {
        public void ThemNgayLe(NgayLe ngayLes)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO NgayLe (ngayles, tenngayle, mota) VALUES (@ngayles, @TenNgayLe, @MoTa)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ngayles", ngayLes.ngayles);
                command.Parameters.AddWithValue("@TenNgayLe", ngayLes.tenngayle);
                command.Parameters.AddWithValue("@MoTa", ngayLes.mota);
                command.ExecuteNonQuery();
            }
        }
        public void CapNhatNgayLe(NgayLe ngayLes)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE NgayLe SET ngayles = @NgayLe, tenngayle = @TenNgayLe, mota = @MoTa WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NgayLe", ngayLes.ngayles);
                command.Parameters.AddWithValue("@TenNgayLe", ngayLes.tenngayle);
                command.Parameters.AddWithValue("@MoTa", ngayLes.mota);
                command.Parameters.AddWithValue("@Id", ngayLes.id);

                command.ExecuteNonQuery();
            }
        }
        public void XoaNgayLe(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM NgayLe WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
        public List<NgayLe> GetAllNgayLes()
        {
            List<NgayLe> ngayLes = new List<NgayLe>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NgayLe";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NgayLe ngayLe = MapNgayLeFromDataReader(reader);
                        ngayLes.Add(ngayLe);
                    }
                }
            }

            return ngayLes;
        }
        public NgayLe GetNgayLesbyNgay(DateTime ngayles)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT CAST(ngayles AS DATE) AS ngayles, tenngayle, mota FROM NgayLe WHERE CAST(ngayles AS DATE) = @ngayles";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ngayles", ngayles);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapNgayLeFromDataReader(reader);
                    }
                }
            }
            return null;
        }
        public NgayLe GetNgayLeById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NgayLe WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapNgayLeFromDataReader(reader);
                    }
                }
            }

            return null;
        }
        private NgayLe MapNgayLeFromDataReader(SqlDataReader reader)
        {
            return new NgayLe
            {
                id = (int)reader["id"],
                ngayles = (DateTime)reader["ngayles"],
                tenngayle = reader["tenngayle"].ToString(),
                mota = reader["mota"].ToString()
            };
        }
    }
}
