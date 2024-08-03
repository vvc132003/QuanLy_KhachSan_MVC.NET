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
    public class GopDonDatPhongService
    {
        public void Create(GopDonDatPhong gopDonDatPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "INSERT INTO GopDonDatPhong (iddatphongcu, iddatphongmoi, tienphong) VALUES (@iddatphongcu, @iddatphongmoi, @tienphong)";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@iddatphongcu", gopDonDatPhong.iddatphongcu);
                cmd.Parameters.AddWithValue("@iddatphongmoi", gopDonDatPhong.iddatphongmoi);
                cmd.Parameters.AddWithValue("@tienphong", gopDonDatPhong.tienphong);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public GopDonDatPhong GetById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM GopDonDatPhong WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GopDonDatPhong
                        {
                            id = (int)reader["id"],
                            iddatphongcu = (int)reader["iddatphongcu"],
                            iddatphongmoi = (int)reader["iddatphongmoi"],
                            tienphong = Convert.ToSingle(reader["tienphong"]) // Convert decimal to float
                        };
                    }
                }
            }
            return null;
        }

        public GopDonDatPhong GetByIdDatPhongMoi(int iddatphongmoi)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM GopDonDatPhong WHERE iddatphongmoi = @iddatphongmoi";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@iddatphongmoi", iddatphongmoi);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GopDonDatPhong
                        {
                            id = (int)reader["id"],
                            iddatphongcu = (int)reader["iddatphongcu"],
                            iddatphongmoi = (int)reader["iddatphongmoi"],
                            tienphong = Convert.ToSingle(reader["tienphong"]) // Convert decimal to float
                        };
                    }
                }
            }
            return null;
        }

        public void Update(GopDonDatPhong gopDonDatPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "UPDATE GopDonDatPhong SET iddatphongcu = @iddatphongcu, iddatphongmoi = @iddatphongmoi, tienphong = @tienphong WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", gopDonDatPhong.id);
                cmd.Parameters.AddWithValue("@iddatphongcu", gopDonDatPhong.iddatphongcu);
                cmd.Parameters.AddWithValue("@iddatphongmoi", gopDonDatPhong.iddatphongmoi);
                cmd.Parameters.AddWithValue("@tienphong", gopDonDatPhong.tienphong);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "DELETE FROM GopDonDatPhong WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<GopDonDatPhong> GetAll()
        {
            List<GopDonDatPhong> gopDonDatPhongs = new List<GopDonDatPhong>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string sql = "SELECT * FROM GopDonDatPhong";
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gopDonDatPhongs.Add(new GopDonDatPhong
                        {
                            id = (int)reader["id"],
                            iddatphongcu = (int)reader["iddatphongcu"],
                            iddatphongmoi = (int)reader["iddatphongmoi"],
                            tienphong = Convert.ToSingle(reader["tienphong"]) // Convert decimal to float
                        });
                    }
                }
            }

            return gopDonDatPhongs;
        }
    }
}
