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
    public class CuocHoiThoaiService : CuocHoiThoaiRepository
    {
        public List<CuocHoiThoai> GetCuocHoiThoaiList()
        {
            List<CuocHoiThoai> cuocHoiThoaiList = new List<CuocHoiThoai>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM CuocHoiThoai";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CuocHoiThoai cuocHoiThoai = new CuocHoiThoai
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Tieude = reader["Tieude"].ToString(),
                            NhanVienTaoid = Convert.ToInt32(reader["NhanVienTaoid"]),
                            LoaiHoiThoai = reader["LoaiHoiThoai"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCaonhatVao = Convert.ToDateTime(reader["DuocCaonhatVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };

                        cuocHoiThoaiList.Add(cuocHoiThoai);
                    }
                }
            }

            return cuocHoiThoaiList;
        }
        public int GetConversationCount(int nhanVienHienTai, int nhanVienDuocChon)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) AS so_luong_cuoc_tro_chuyen FROM CuocHoiThoai WHERE EXISTS (SELECT 1 FROM NguoiThamGia WHERE CuocHoiThoai.id = NguoiThamGia.cuochoithoaiid AND NguoiThamGia.nhanvienthamgiaid = @nhanVienHienTai) AND EXISTS (SELECT 1 FROM NguoiThamGia WHERE CuocHoiThoai.id = NguoiThamGia.cuochoithoaiid AND NguoiThamGia.nhanvienthamgiaid = @nhanVienDuocChon) AND CuocHoiThoai.loaihoithoai = '1-1'", connection))
                {
                    command.Parameters.AddWithValue("@nhanVienHienTai", nhanVienHienTai);
                    command.Parameters.AddWithValue("@nhanVienDuocChon", nhanVienDuocChon);
                    return (int)command.ExecuteScalar();
                }
            }
        }
        public List<CuocHoiThoai> GetCuocHoiThoaiListById(int nhanvienthamgiaid)
        {
            List<CuocHoiThoai> cuocHoiThoaiList = new List<CuocHoiThoai>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = @"
                                    SELECT ch.id, ch.tieude, ch.duoctaovao,ch.loaihoithoai
                                    FROM CuocHoiThoai ch
                                    JOIN NguoiThamGia nt ON ch.id = nt.cuochoithoaiid
                                    WHERE nt.nhanvienthamgiaid = @nhanvienthamgiaid
                                    ORDER BY ch.duoctaovao DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nhanvienthamgiaid", nhanvienthamgiaid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CuocHoiThoai cuocHoiThoai = new CuocHoiThoai
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Tieude = reader["Tieude"].ToString(),
                            LoaiHoiThoai = reader["LoaiHoiThoai"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                        };

                        cuocHoiThoaiList.Add(cuocHoiThoai);
                    }
                }
            }

            return cuocHoiThoaiList;
        }
        public CuocHoiThoai GetCuocHoiThoaiById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM CuocHoiThoai WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CuocHoiThoai
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Tieude = reader["Tieude"].ToString(),
                            NhanVienTaoid = Convert.ToInt32(reader["NhanVienTaoid"]),
                            LoaiHoiThoai = reader["LoaiHoiThoai"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCaonhatVao = Convert.ToDateTime(reader["DuocCaonhatVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }
        public async Task<CuocHoiThoai> GetCuocHoiThoaiByIdHub(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM CuocHoiThoai WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CuocHoiThoai
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Tieude = reader["Tieude"].ToString(),
                            NhanVienTaoid = Convert.ToInt32(reader["NhanVienTaoid"]),
                            LoaiHoiThoai = reader["LoaiHoiThoai"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DuocCaonhatVao = Convert.ToDateTime(reader["DuocCaonhatVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };
                    }
                }
            }
            return null; // If no record is found
        }
        public void AddCuocHoiThoai(CuocHoiThoai cuocHoiThoai)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO CuocHoiThoai (Tieude, NhanVienTaoid, LoaiHoiThoai, DuocTaoVao, DuocCaonhatVao, DaXoaVao) " +
                               "VALUES (@Tieude, @NhanVienTaoid, @LoaiHoiThoai, @DuocTaoVao, @DuocCaonhatVao, @DaXoaVao)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Tieude", cuocHoiThoai.Tieude);
                command.Parameters.AddWithValue("@NhanVienTaoid", cuocHoiThoai.NhanVienTaoid);
                command.Parameters.AddWithValue("@LoaiHoiThoai", cuocHoiThoai.LoaiHoiThoai);
                command.Parameters.AddWithValue("@DuocTaoVao", cuocHoiThoai.DuocTaoVao);
                command.Parameters.AddWithValue("@DuocCaonhatVao", cuocHoiThoai.DuocCaonhatVao);
                command.Parameters.AddWithValue("@DaXoaVao", cuocHoiThoai.DaXoaVao);

                command.ExecuteNonQuery();
            }
        }
        public int AddCuocHoiThoaiByid(CuocHoiThoai cuocHoiThoai)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO CuocHoiThoai (Tieude, NhanVienTaoid, LoaiHoiThoai, DuocTaoVao, DuocCaonhatVao, DaXoaVao) " +
                               "VALUES (@Tieude, @NhanVienTaoid, @LoaiHoiThoai, @DuocTaoVao, @DuocCaonhatVao, @DaXoaVao);" +
                               "SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Tieude", cuocHoiThoai.Tieude);
                    command.Parameters.AddWithValue("@NhanVienTaoid", cuocHoiThoai.NhanVienTaoid);
                    command.Parameters.AddWithValue("@LoaiHoiThoai", cuocHoiThoai.LoaiHoiThoai);
                    command.Parameters.AddWithValue("@DuocTaoVao", cuocHoiThoai.DuocTaoVao);
                    command.Parameters.AddWithValue("@DuocCaonhatVao", cuocHoiThoai.DuocCaonhatVao);
                    command.Parameters.AddWithValue("@DaXoaVao", cuocHoiThoai.DaXoaVao);
                    int insertedId = Convert.ToInt32(command.ExecuteScalar());

                    return insertedId;
                }
            }
        }

        public void UpdateCuocHoiThoai(CuocHoiThoai cuocHoiThoai)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE CuocHoiThoai SET Tieude = @Tieude, NhanVienTaoid = @NhanVienTaoid, " +
                               "LoaiHoiThoai = @LoaiHoiThoai, DuocTaoVao = @DuocTaoVao, DuocCaonhatVao = @DuocCaonhatVao, " +
                               "DaXoaVao = @DaXoaVao WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", cuocHoiThoai.Id);
                command.Parameters.AddWithValue("@Tieude", cuocHoiThoai.Tieude);
                command.Parameters.AddWithValue("@NhanVienTaoid", cuocHoiThoai.NhanVienTaoid);
                command.Parameters.AddWithValue("@LoaiHoiThoai", cuocHoiThoai.LoaiHoiThoai);
                command.Parameters.AddWithValue("@DuocTaoVao", cuocHoiThoai.DuocTaoVao);
                command.Parameters.AddWithValue("@DuocCaonhatVao", cuocHoiThoai.DuocCaonhatVao);
                command.Parameters.AddWithValue("@DaXoaVao", cuocHoiThoai.DaXoaVao);

                command.ExecuteNonQuery();
            }
        }
        public async Task UpdateCuocHoiThoaiHub(CuocHoiThoai cuocHoiThoai)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = @"UPDATE CuocHoiThoai SET Tieude = @Tieude,
                                DuocTaoVao = GETDATE()
                                WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", cuocHoiThoai.Id);
                command.Parameters.AddWithValue("@Tieude", cuocHoiThoai.Tieude);
                command.Parameters.AddWithValue("@DuocTaoVao", cuocHoiThoai.DuocTaoVao);
                int rowsAffected = await command.ExecuteNonQueryAsync();
            }
        }

        public void DeleteCuocHoiThoai(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM CuocHoiThoai WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
