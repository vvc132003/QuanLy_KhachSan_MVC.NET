using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;
using System.Globalization;

namespace  Service
{
    public class ThoiGianService : ThoiGianRepository
    {
        public void CapNhatThoiGian(ThoiGian thoiGian)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE ThoiGian SET thoigiannhanphong=@thoigiannhanphong,phuthunhanphong=@phuthunhanphong, thoigianra = @thoigianra,phuthutraphong=@phuthutraphong, mota = @mota WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@thoigiannhanphong", thoiGian.thoigiannhanphong);
                    command.Parameters.AddWithValue("@phuthunhanphong", thoiGian.phuthunhanphong);
                    command.Parameters.AddWithValue("@thoigianra", thoiGian.thoigianra);
                    command.Parameters.AddWithValue("@phuthutraphong", thoiGian.phuthutraphong);
                    command.Parameters.AddWithValue("@mota", thoiGian.mota);
                    command.Parameters.AddWithValue("@id", thoiGian.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ThoiGian> GetAllThoiGian()
        {
            List<ThoiGian> danhSachThoiGian = new List<ThoiGian>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM ThoiGian";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian
                    {
                        id = Convert.ToInt32(reader["id"]),
                        thoigiannhanphong = TimeSpan.ParseExact(reader["thoigiannhanphong"].ToString(), "HH:mm", CultureInfo.InvariantCulture),
                        phuthunhanphong = (float)reader["phuthunhanphong"],
                        thoigianra = TimeSpan.ParseExact(reader["thoigianra"].ToString(), "HH:mm", CultureInfo.InvariantCulture),
                        phuthutraphong = (float)reader["phuthutraphong"],
                        mota = Convert.ToString(reader["mota"]),
                    };
                    danhSachThoiGian.Add(thoiGian);
                }
                reader.Close();
            }

            return danhSachThoiGian;
        }

        public ThoiGian GetThoiGian(int idkhachsan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThoiGian WHERE idkhachsan=@idkhachsan ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idkhachsan", idkhachsan);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian();
                    thoiGian.id = Convert.ToInt32(reader["id"]);
                    thoiGian.mota = Convert.ToString(reader["mota"]);
                    TimeSpan thoigiannhanphong;
                    if (TimeSpan.TryParseExact(reader["thoigiannhanphong"].ToString(), "hh\\:mm", CultureInfo.InvariantCulture, out thoigiannhanphong))
                    {
                        thoiGian.thoigiannhanphong = thoigiannhanphong;
                    }
                    else
                    {

                    }
                    TimeSpan thoigianra;
                    if (TimeSpan.TryParseExact(reader["thoigianra"].ToString(), "hh\\:mm", CultureInfo.InvariantCulture, out thoigianra))
                    {
                        thoiGian.thoigianra = thoigianra;
                    }
                    else
                    {
                    }
                    float phuthunhanphong;
                    if (float.TryParse(reader["phuthunhanphong"].ToString(), out phuthunhanphong))
                    {
                        thoiGian.phuthunhanphong = phuthunhanphong;
                    }
                    else
                    {
                    }
                    float phuthutraphong;
                    if (float.TryParse(reader["phuthutraphong"].ToString(), out phuthutraphong))
                    {
                        thoiGian.phuthutraphong = phuthutraphong;
                    }
                    else
                    {
                    }
                    return thoiGian;
                }
                else
                {
                    return null;
                }

            }
        }
        public ThoiGian GetThoiGianById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThoiGian WHERE id = @id ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ThoiGian thoiGian = new ThoiGian();
                    thoiGian.id = Convert.ToInt32(reader["id"]);
                    thoiGian.mota = Convert.ToString(reader["mota"]);
                    TimeSpan thoigiannhanphong;
                    if (TimeSpan.TryParseExact(reader["thoigiannhanphong"].ToString(), "hh\\:mm", CultureInfo.InvariantCulture, out thoigiannhanphong))
                    {
                        thoiGian.thoigiannhanphong = thoigiannhanphong;
                    }
                    else
                    {

                    }
                    TimeSpan thoigianra;
                    if (TimeSpan.TryParseExact(reader["thoigianra"].ToString(), "hh\\:mm", CultureInfo.InvariantCulture, out thoigianra))
                    {
                        thoiGian.thoigianra = thoigianra;
                    }
                    else
                    {
                    }
                    float phuthunhanphong;
                    if (float.TryParse(reader["phuthunhanphong"].ToString(), out phuthunhanphong))
                    {
                        thoiGian.phuthunhanphong = phuthunhanphong;
                    }
                    else
                    {
                    }
                    float phuthutraphong;
                    if (float.TryParse(reader["phuthutraphong"].ToString(), out phuthutraphong))
                    {
                        thoiGian.phuthutraphong = phuthutraphong;
                    }
                    else
                    {
                    }
                    return thoiGian;
                }
                else
                {
                    return null;
                }
            }
        }

        public void ThemThoiGian(ThoiGian thoiGian)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThoiGian (thoigiannhanphong, phuthunhanphong, thoigianra, phuthutraphong, mota, idkhachsan) " +
                               "VALUES (@thoigiannhanphong, @phuthunhanphong, @thoigianra, @phuthutraphong, @mota, @idkhachsan)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@thoigiannhanphong", thoiGian.thoigiannhanphong);
                    command.Parameters.AddWithValue("@phuthunhanphong", thoiGian.phuthunhanphong);
                    command.Parameters.AddWithValue("@thoigianra", thoiGian.thoigianra);
                    command.Parameters.AddWithValue("@phuthutraphong", thoiGian.phuthutraphong);
                    command.Parameters.AddWithValue("@mota", thoiGian.mota);
                    command.Parameters.AddWithValue("@idkhachsan", thoiGian.idkhachsan);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}