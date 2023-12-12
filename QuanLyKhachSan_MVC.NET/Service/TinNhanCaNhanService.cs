using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class TinNhanCaNhanService : TinNhanCaNhanRepository
    {
        public List<TinNhanCaNhan> Gettinnhancanhantheoidnhanvien(int idnhanviengui, int idnhanviennhan)
        {
            List<TinNhanCaNhan> messages = new List<TinNhanCaNhan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM TinNhanCaNhan " +
                       "WHERE (idnhanviengui = @idnhanviengui AND idnhanviennhan = @idnhanviennhan) OR (idnhanviengui = @idnhanviennhan AND idnhanviennhan = @idnhanviengui) " +
                       "ORDER BY thoigiangui";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnhanviengui", idnhanviengui);
                    command.Parameters.AddWithValue("@idnhanviennhan", idnhanviennhan);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TinNhanCaNhan message = new TinNhanCaNhan()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            idnhanviengui = Convert.ToInt32(reader["idnhanviengui"]),
                            idnhanviennhan = Convert.ToInt32(reader["idnhanviennhan"]),
                            noidung = reader["noidung"].ToString(),
                            thoigiangui = Convert.ToDateTime(reader["thoigiangui"]),
                        };
                        messages.Add(message);
                    }

                    reader.Close();
                }
            }

            return messages;
        }

        public void ThemTinNhanCaNhan(TinNhanCaNhan tinNhanCaNhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO TinNhanCaNhan (idnhanviengui, idnhanviennhan, noidung, thoigiangui) VALUES (@idnhanviengui, @idnhanviennhan, @noidung, @thoigiangui)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnhanviengui", tinNhanCaNhan.idnhanviengui);
                    command.Parameters.AddWithValue("@idnhanviennhan", tinNhanCaNhan.idnhanviennhan);
                    command.Parameters.AddWithValue("@noidung", tinNhanCaNhan.noidung);
                    command.Parameters.AddWithValue("@thoigiangui", tinNhanCaNhan.thoigiangui);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}