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
    public class GiamGiaNgayLeService : GiamGiaNgayLeRepository
    {
        public void DeleteGiamGiaNgayLe(int id)
        {
            throw new NotImplementedException();
        }

        public void EditGiamGiaNgayLe(GiamGiaNgayLe giamGiaNgayLe)
        {
            string query = "UPDATE GiamGiaNgayLe SET ngayle = @ngayle, ngaybatdau = @ngaybatdau, ngayketthuc = @ngayketthuc, tenngayle = @tenngayle, dieuchinhgiaphong = @dieuchinhgiaphong, dieuchinhgiasanpham = @dieuchinhgiasanpham WHERE id = @id";

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngayle", giamGiaNgayLe.ngayle);
                    command.Parameters.AddWithValue("@ngaybatdau", giamGiaNgayLe.ngaybatdau);
                    command.Parameters.AddWithValue("@ngayketthuc", giamGiaNgayLe.ngayketthuc);
                    command.Parameters.AddWithValue("@tenngayle", giamGiaNgayLe.tenngayle);
                    command.Parameters.AddWithValue("@dieuchinhgiaphong", giamGiaNgayLe.dieuchinhgiaphong);
                    command.Parameters.AddWithValue("@dieuchinhgiasanpham", giamGiaNgayLe.dieuchinhgiasanpham);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<GiamGiaNgayLe> GetAllGiamGiaNgayLe()
        {
            List<GiamGiaNgayLe> giamGiaNgayLes = new List<GiamGiaNgayLe>();
            string query = "SELECT * FROM GiamGiaNgayLe";

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GiamGiaNgayLe giamGia = new GiamGiaNgayLe
                            {
                                id = Convert.ToInt32(reader["id"]),
                                ngayle = Convert.ToDateTime(reader["ngayle"]),
                                ngaybatdau = Convert.ToDateTime(reader["ngaybatdau"]),
                                ngayketthuc = Convert.ToDateTime(reader["ngayketthuc"]),
                                tenngayle = reader["tenngayle"].ToString(),
                                dieuchinhgiaphong = Convert.ToSingle(reader["dieuchinhgiaphong"]),
                                dieuchinhgiasanpham = Convert.ToSingle(reader["dieuchinhgiasanpham"])
                            };
                            giamGiaNgayLes.Add(giamGia);
                        }
                    }
                }
            }
            return giamGiaNgayLes;
        }

        public GiamGiaNgayLe GetGiamGiaNgayLeById(int id)
        {
            GiamGiaNgayLe result = null;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM GiamGiaNgayLe WHERE id = @id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new GiamGiaNgayLe
                    {
                        id = Convert.ToInt32(reader["id"]),
                        ngayle = Convert.ToDateTime(reader["ngayle"]),
                        ngaybatdau = Convert.ToDateTime(reader["ngaybatdau"]),
                        ngayketthuc = Convert.ToDateTime(reader["ngayketthuc"]),
                        tenngayle = reader["tenngayle"].ToString(),
                        dieuchinhgiaphong = Convert.ToSingle(reader["dieuchinhgiaphong"]),
                        dieuchinhgiasanpham = Convert.ToSingle(reader["dieuchinhgiasanpham"])
                    };
                }
            }
            return result;
        }


        public GiamGiaNgayLe GetGiamGiaNgayLeByNgayLe(DateTime ngayLe)
        {
            GiamGiaNgayLe result = null;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM GiamGiaNgayLe WHERE ngayle = @ngayle", connection);
                cmd.Parameters.AddWithValue("@NgayLe", ngayLe);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new GiamGiaNgayLe
                    {
                        id = Convert.ToInt32(reader["id"]),
                        ngayle = Convert.ToDateTime(reader["ngayle"]),
                        ngaybatdau = Convert.ToDateTime(reader["ngaybatdau"]),
                        ngayketthuc = Convert.ToDateTime(reader["ngayketthuc"]),
                        tenngayle = reader["tenngayle"].ToString(),
                        dieuchinhgiaphong = Convert.ToSingle(reader["dieuchinhgiaphong"]),
                        dieuchinhgiasanpham = Convert.ToSingle(reader["dieuchinhgiasanpham"])
                    };
                }
            }
            return result;
        }


        public void ThemGiamGiaNgayLe(GiamGiaNgayLe giamGiaNgayLe)
        {
            string query = "INSERT INTO GiamGiaNgayLe (ngayle, ngaybatdau, ngayketthuc, tenngayle, dieuchinhgiaphong, dieuchinhgiasanpham) VALUES (@ngayle, @ngaybatdau, @ngayketthuc, @tenngayle, @dieuchinhgiaphong, @dieuchinhgiasanpham)";

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ngayle", giamGiaNgayLe.ngayle);
                    command.Parameters.AddWithValue("@ngaybatdau", giamGiaNgayLe.ngaybatdau);
                    command.Parameters.AddWithValue("@ngayketthuc", giamGiaNgayLe.ngayketthuc);
                    command.Parameters.AddWithValue("@tenngayle", giamGiaNgayLe.tenngayle);
                    command.Parameters.AddWithValue("@dieuchinhgiaphong", giamGiaNgayLe.dieuchinhgiaphong);
                    command.Parameters.AddWithValue("@dieuchinhgiasanpham", giamGiaNgayLe.dieuchinhgiasanpham);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
