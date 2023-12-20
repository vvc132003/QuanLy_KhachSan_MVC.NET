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
    public class SuDungMaGiamGiaService : SuDungMaGiamGiaRepository
    {
        public void ThemSuDungMaGiamGia(SuDungMaGiamGia suDungMaGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO SuDungMaGiamGia (idmagiamgia, iddatphong, ngaysudung) VALUES (@idmagiamgia, @iddatphong, @ngaysudung)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idmagiamgia", suDungMaGiamGia.idmagiamgia);
                command.Parameters.AddWithValue("@iddatphong", suDungMaGiamGia.iddatphong);
                command.Parameters.AddWithValue("@ngaysudung", suDungMaGiamGia.ngaysudung);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void XoaSuDungMaGiamGia(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM SuDungMaGiamGia WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void CapNhatSuDungMaGiamGia(SuDungMaGiamGia suDungMaGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "UPDATE SuDungMaGiamGia SET idmagiamgia = @idmagiamgia, iddatphong = @iddatphong, ngaysudung = @ngaysudung WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idmagiamgia", suDungMaGiamGia.idmagiamgia);
                command.Parameters.AddWithValue("@iddatphong", suDungMaGiamGia.iddatphong);
                command.Parameters.AddWithValue("@ngaysudung", suDungMaGiamGia.ngaysudung);
                command.Parameters.AddWithValue("@id", suDungMaGiamGia.id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<SuDungMaGiamGia> GetAllSuDungMaGiamGia()
        {
            List<SuDungMaGiamGia> listSuDungMaGiamGia = new List<SuDungMaGiamGia>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM SuDungMaGiamGia";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia
                        {
                            id = Convert.ToInt32(reader["id"]),
                            idmagiamgia = Convert.ToInt32(reader["idmagiamgia"]),
                            iddatphong = Convert.ToInt32(reader["iddatphong"]),
                            ngaysudung = Convert.ToDateTime(reader["ngaysudung"])
                        };

                        listSuDungMaGiamGia.Add(suDungMaGiamGia);
                    }
                }
            }

            return listSuDungMaGiamGia;
        }

        public SuDungMaGiamGia GetSuDungMaGiamGiaById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM SuDungMaGiamGia WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia
                        {
                            id = Convert.ToInt32(reader["id"]),
                            idmagiamgia = Convert.ToInt32(reader["idmagiamgia"]),
                            iddatphong = Convert.ToInt32(reader["iddatphong"]),
                            ngaysudung = Convert.ToDateTime(reader["ngaysudung"])
                        };

                        return suDungMaGiamGia;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public SuDungMaGiamGia GetSuDungMaGiamGiaByIddatphong(int iddatphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM SuDungMaGiamGia WHERE iddatphong  = @iddatphong  ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@iddatphong ", iddatphong);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia
                        {
                            id = Convert.ToInt32(reader["id"]),
                            idmagiamgia = Convert.ToInt32(reader["idmagiamgia"]),
                            iddatphong = Convert.ToInt32(reader["iddatphong"]),
                            ngaysudung = Convert.ToDateTime(reader["ngaysudung"])
                        };

                        return suDungMaGiamGia;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
