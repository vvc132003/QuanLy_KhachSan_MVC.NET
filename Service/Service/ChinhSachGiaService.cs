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
    public class ChinhSachGiaService : ChinhSachGiaRepository
    {
        public void ThemChinhSachGia(ChinhSachGia chinhSachGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ChinhSachGia (tenchinhsach, ngaybatdau, ngayketthuc, idngayle, dieuchinhgiaphong) " +
                               "VALUES (@TenChinhSach, @NgayBatDau, @NgayKetThuc, @IdNgayLe, @dieuchinhgiaphong)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenChinhSach", chinhSachGia.tenchinhsach);
                command.Parameters.AddWithValue("@NgayBatDau", chinhSachGia.ngaybatdau);
                command.Parameters.AddWithValue("@NgayKetThuc", chinhSachGia.ngayketthuc);
                command.Parameters.AddWithValue("@IdNgayLe", chinhSachGia.idngayle);
                command.Parameters.AddWithValue("@dieuchinhgiaphong", chinhSachGia.dieuchinhgiaphong);
                command.ExecuteNonQuery();
            }
        }

        public void CapNhatChinhSachGia(ChinhSachGia chinhSachGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ChinhSachGia SET tenchinhsach = @TenChinhSach, ngaybatdau = @NgayBatDau, " +
                               "ngayketthuc = @NgayKetThuc, idngayle = @IdNgayLe, " +
                               "dieuchinhgiaphong = @dieuchinhgiaphong WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenChinhSach", chinhSachGia.tenchinhsach);
                command.Parameters.AddWithValue("@NgayBatDau", chinhSachGia.ngaybatdau);
                command.Parameters.AddWithValue("@NgayKetThuc", chinhSachGia.ngayketthuc);
                command.Parameters.AddWithValue("@IdNgayLe", chinhSachGia.idngayle);
                command.Parameters.AddWithValue("@dieuchinhgiaphong", chinhSachGia.dieuchinhgiaphong);
                command.Parameters.AddWithValue("@Id", chinhSachGia.id);
                command.ExecuteNonQuery();
            }
        }

        public void XoaChinhSachGia(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM ChinhSachGia WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public List<ChinhSachGia> GetAllChinhSachGia()
        {
            List<ChinhSachGia> chinhSachGias = new List<ChinhSachGia>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM ChinhSachGia";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ChinhSachGia chinhSachGia = MapChinhSachGiaFromDataReader(reader);
                        chinhSachGias.Add(chinhSachGia);
                    }
                }
            }

            return chinhSachGias;
        }

        public ChinhSachGia GetChinhSachGiaById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM ChinhSachGia WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapChinhSachGiaFromDataReader(reader);
                    }
                }
            }

            return null;
        }

        public ChinhSachGia GetChinhSachGiaByIdngayle(int idngayle)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM ChinhSachGia WHERE idngayle = @idngayle";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idngayle", idngayle);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapChinhSachGiaFromDataReader(reader);
                    }
                }
            }

            return null;
        }


        private ChinhSachGia MapChinhSachGiaFromDataReader(SqlDataReader reader)
        {
            return new ChinhSachGia
            {
                id = (int)reader["id"],
                tenchinhsach = reader["tenchinhsach"].ToString(),
                ngaybatdau = (DateTime)reader["ngaybatdau"],
                ngayketthuc = (DateTime)reader["ngayketthuc"],
                idngayle = (int)reader["idngayle"],
                dieuchinhgiaphong = Convert.ToSingle(reader["dieuchinhgiaphong"]),
            };
        }
    }
}