using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace  Service
{
    public class ThietBiService : ThietBiRepository
    {
        public void CapNhatThietBi(ThietBi thietBi)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ThietBi SET tenthietbi = @tenthietbi, giathietbi = @giathietbi, " +
                               "ngaymua = @ngaymua, soluongcon = @soluongcon, image = @image, mota = @mota " +
                               "WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenthietbi", thietBi.tenthietbi);
                    command.Parameters.AddWithValue("@giathietbi", thietBi.giathietbi);
                    command.Parameters.AddWithValue("@ngaymua", thietBi.ngaymua);
                    command.Parameters.AddWithValue("@soluongcon", thietBi.soluongcon);
                    command.Parameters.AddWithValue("@image", thietBi.image);
                    command.Parameters.AddWithValue("@mota", thietBi.mota);
                    command.Parameters.AddWithValue("@id", thietBi.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ThietBi> GetAllThietBi()
        {
            List<ThietBi> thietBiList = new List<ThietBi>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThietBi";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThietBi thietBi = new ThietBi()
                            {
                                id = (int)reader["id"],
                                tenthietbi = reader["tenthietbi"].ToString(),
                                giathietbi = Convert.ToSingle(reader["giathietbi"]),
                                ngaymua = (DateTime)reader["ngaymua"],
                                soluongcon = (int)reader["soluongcon"],
                                image = reader["image"].ToString(),
                                mota = reader["mota"].ToString()
                            };
                            thietBiList.Add(thietBi);
                        }
                    }
                }
                return thietBiList;
            }
        }

        public ThietBi GetThietBiByID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThietBi WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ThietBi thietBi = new ThietBi
                            {
                                id = (int)reader["id"],
                                tenthietbi = reader["tenthietbi"].ToString(),
                                giathietbi = Convert.ToSingle(reader["giathietbi"]),
                                ngaymua = (DateTime)reader["ngaymua"],
                                soluongcon = (int)reader["soluongcon"],
                                image = reader["image"].ToString(),
                                mota = reader["mota"].ToString()
                            };
                            return thietBi;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void ThemThietBi(ThietBi thietBi)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThietBi (tenthietbi, giathietbi, ngaymua, soluongcon, image, mota) " +
                               "VALUES (@tenthietbi, @giathietbi, @ngaymua, @soluongcon, @image, @MoTa)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenthietbi", thietBi.tenthietbi);
                    command.Parameters.AddWithValue("@giathietbi", thietBi.giathietbi);
                    command.Parameters.AddWithValue("@ngaymua", thietBi.ngaymua);
                    command.Parameters.AddWithValue("@soluongcon", thietBi.soluongcon);
                    command.Parameters.AddWithValue("@image", thietBi.image);
                    command.Parameters.AddWithValue("@mota", thietBi.mota);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaThietBi(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM ThietBi WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
