using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class ViTriBoPhanService : ViTriBoPhanRepository
    {
        public void CapNhatViTriBoPhan(ViTriBoPhan viTriBoPhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ViTriBoPhan SET tenvitri = @tenvitri, mota = @mota, luong = @luong   WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenvitri", viTriBoPhan.tenvitri);
                    command.Parameters.AddWithValue("@mota", viTriBoPhan.mota);
                    command.Parameters.AddWithValue("@luong", viTriBoPhan.luong);
                    command.Parameters.AddWithValue("@id", viTriBoPhan.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ViTriBoPhan> GetAllViTriBoPhan()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<ViTriBoPhan> viTriBoPhans = new List<ViTriBoPhan>();
                connection.Open();
                string query = "select * from ViTriBoPhan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViTriBoPhan viTriBoPhan = new ViTriBoPhan()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenvitri = reader["tenvitri"].ToString(),
                                mota = reader["mota"].ToString(),
                                luong = Convert.ToSingle(reader["luong"]),
                                idbophan = Convert.ToInt32(reader["idbophan"]),
                            };
                            viTriBoPhans.Add(viTriBoPhan);
                        }
                    }
                }
                return viTriBoPhans;
            }
        }

        public ViTriBoPhan GetViTriBoPhanID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from ViTriBoPhan where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ViTriBoPhan viTriBoPhan = new ViTriBoPhan
                            {
                                id = Convert.ToInt32(reader["id"]),
                                tenvitri = reader["tenvitri"].ToString(),
                                mota = reader["mota"].ToString(),
                                luong = Convert.ToSingle(reader["luong"]),

                            };
                            return viTriBoPhan;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemViTriBoPhan(ViTriBoPhan viTriBoPhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ViTriBoPhan (tenvitri,mota,luong,idbophan) VALUES (@tenvitri,@mota,@luong,@idbophan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenvitri", viTriBoPhan.tenvitri);
                    command.Parameters.AddWithValue("@mota", viTriBoPhan.mota);
                    command.Parameters.AddWithValue("@luong", viTriBoPhan.luong);
                    command.Parameters.AddWithValue("@idbophan", viTriBoPhan.idbophan);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaViTriBoPhan(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM ViTriBoPhan WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}