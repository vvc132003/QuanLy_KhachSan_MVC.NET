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

namespace Service
{
    public class NguoiThamGiaService : NguoiThamGiaRepository
    {
        public List<NguoiThamGia> GetNguoiThamGiaList()
        {
            List<NguoiThamGia> nguoiThamGiaList = new List<NguoiThamGia>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NguoiThamGia";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NguoiThamGia nguoiThamGia = new NguoiThamGia
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienThamGiaId = Convert.ToInt32(reader["NhanVienThamGiaId"]),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCapNhatVao = Convert.ToDateTime(reader["DuocCapNhatVao"])
                        };

                        nguoiThamGiaList.Add(nguoiThamGia);
                    }
                }
            }

            return nguoiThamGiaList;
        }
        public List<NguoiThamGia> GetNguoiThamGiaListById(int CuocHoiThoaiId)
        {
            List<NguoiThamGia> nguoiThamGiaList = new List<NguoiThamGia>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NguoiThamGia WHERE CuocHoiThoaiId = @CuocHoiThoaiId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CuocHoiThoaiId", CuocHoiThoaiId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NguoiThamGia nguoiThamGia = new NguoiThamGia
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienThamGiaId = Convert.ToInt32(reader["NhanVienThamGiaId"]),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCapNhatVao = Convert.ToDateTime(reader["DuocCapNhatVao"])
                        };

                        nguoiThamGiaList.Add(nguoiThamGia);
                    }
                }
            }

            return nguoiThamGiaList;
        }
        public NguoiThamGia GetNguoiThamGiaById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NguoiThamGia WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NguoiThamGia
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienThamGiaId = Convert.ToInt32(reader["NhanVienThamGiaId"]),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCapNhatVao = Convert.ToDateTime(reader["DuocCapNhatVao"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }
        public void AddNguoiThamGia(NguoiThamGia nguoiThamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO NguoiThamGia (CuocHoiThoaiId, NhanVienThamGiaId, DuocTaoVao, DuocCapNhatVao) " +
                               "VALUES (@CuocHoiThoaiId, @NhanVienThamGiaId, @DuocTaoVao, @DuocCapNhatVao)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CuocHoiThoaiId", nguoiThamGia.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienThamGiaId", nguoiThamGia.NhanVienThamGiaId);
                command.Parameters.AddWithValue("@DuocTaoVao", nguoiThamGia.DuocTaoVao);
                command.Parameters.AddWithValue("@DuocCapNhatVao", nguoiThamGia.DuocCapNhatVao);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateNguoiThamGia(NguoiThamGia nguoiThamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE NguoiThamGia SET CuocHoiThoaiId = @CuocHoiThoaiId, " +
                               "NhanVienThamGiaId = @NhanVienThamGiaId, DuocTaoVao = @DuocTaoVao, " +
                               "DuocCapNhatVao = @DuocCapNhatVao WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", nguoiThamGia.Id);
                command.Parameters.AddWithValue("@CuocHoiThoaiId", nguoiThamGia.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienThamGiaId", nguoiThamGia.NhanVienThamGiaId);
                command.Parameters.AddWithValue("@DuocTaoVao", nguoiThamGia.DuocTaoVao);
                command.Parameters.AddWithValue("@DuocCapNhatVao", nguoiThamGia.DuocCapNhatVao);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteNguoiThamGia(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM NguoiThamGia WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
