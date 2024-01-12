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

namespace Service.Service
{
    public class TinNhanService : TinNhanRepository
    {
        public List<TinNhan> GetTinNhanList()
        {
            List<TinNhan> tinNhanList = new List<TinNhan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM TinNhan";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TinNhan tinNhan = new TinNhan
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienGuiId = Convert.ToInt32(reader["NhanVienGuiId"]),
                            LoaiTinNhan = reader["LoaiTinNhan"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };

                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }
        public List<TinNhan> GetTinNhanListByIdCuocTroChuyen(int cuochoithoaiid)
        {
            List<TinNhan> tinNhanList = new List<TinNhan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM TinNhan WHERE CuocHoiThoaiId = @CuocHoiThoaiId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CuocHoiThoaiId", cuochoithoaiid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TinNhan tinNhan = new TinNhan
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienGuiId = Convert.ToInt32(reader["NhanVienGuiId"]),
                            LoaiTinNhan = reader["LoaiTinNhan"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };

                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }

        public TinNhan GetTinNhanById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM TinNhan WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TinNhan
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienGuiId = Convert.ToInt32(reader["NhanVienGuiId"]),
                            LoaiTinNhan = reader["LoaiTinNhan"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }
        public TinNhan GetTinNhanByIdHoithoaitinnhanmoinhat(int cuochoithoaiid)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = @"
                    SELECT TN.*
                    FROM TinNhan TN
                    WHERE TN.cuochoithoaiid = @cuochoithoaiid
                    AND TN.duoctaovao = (
                        SELECT MAX(duoctaovao)
                        FROM TinNhan
                        WHERE cuochoithoaiid = @cuochoithoaiid
                    );
                    ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cuochoithoaiid", cuochoithoaiid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TinNhan
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CuocHoiThoaiId = Convert.ToInt32(reader["CuocHoiThoaiId"]),
                            NhanVienGuiId = Convert.ToInt32(reader["NhanVienGuiId"]),
                            LoaiTinNhan = reader["LoaiTinNhan"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            DuocTaoVao = Convert.ToDateTime(reader["DuocTaoVao"]),
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"])
                        };
                    }
                }
            }

            return null; // If no record is found
        }
        public void AddTinNhan(TinNhan tinNhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO TinNhan (CuocHoiThoaiId, NhanVienGuiId, LoaiTinNhan, NoiDung, DuocTaoVao, DaXoaVao) " +
                               "VALUES (@CuocHoiThoaiId, @NhanVienGuiId, @LoaiTinNhan, @NoiDung, @DuocTaoVao, @DaXoaVao)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CuocHoiThoaiId", tinNhan.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienGuiId", tinNhan.NhanVienGuiId);
                command.Parameters.AddWithValue("@LoaiTinNhan", tinNhan.LoaiTinNhan);
                command.Parameters.AddWithValue("@NoiDung", tinNhan.NoiDung);
                command.Parameters.AddWithValue("@DuocTaoVao", tinNhan.DuocTaoVao);
                command.Parameters.AddWithValue("@DaXoaVao", tinNhan.DaXoaVao);

                command.ExecuteNonQuery();
            }
        }
        public async Task AddTinNhanBuh(int cuochoithoaiid, int nhanvienguiid, string noidung)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO TinNhan (CuocHoiThoaiId, NhanVienGuiId, LoaiTinNhan, NoiDung, DuocTaoVao, DaXoaVao) " +
                               "VALUES (@CuocHoiThoaiId, @NhanVienGuiId, @LoaiTinNhan, @NoiDung, GETDATE(), GETDATE())";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CuocHoiThoaiId", cuochoithoaiid);
                command.Parameters.AddWithValue("@NhanVienGuiId", nhanvienguiid);
                command.Parameters.AddWithValue("@LoaiTinNhan", "vip");
                command.Parameters.AddWithValue("@NoiDung", noidung);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in AddTinNhanBuh: {ex}");
                    // Log the exception or handle it appropriately
                }
            }
        }
        public void UpdateTinNhan(TinNhan tinNhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE TinNhan SET CuocHoiThoaiId = @CuocHoiThoaiId, " +
                               "NhanVienGuiId = @NhanVienGuiId, LoaiTinNhan = @LoaiTinNhan, " +
                               "NoiDung = @NoiDung, DuocTaoVao = @DuocTaoVao, DaXoaVao = @DaXoaVao " +
                               "WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", tinNhan.Id);
                command.Parameters.AddWithValue("@CuocHoiThoaiId", tinNhan.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienGuiId", tinNhan.NhanVienGuiId);
                command.Parameters.AddWithValue("@LoaiTinNhan", tinNhan.LoaiTinNhan);
                command.Parameters.AddWithValue("@NoiDung", tinNhan.NoiDung);
                command.Parameters.AddWithValue("@DuocTaoVao", tinNhan.DuocTaoVao);
                command.Parameters.AddWithValue("@DaXoaVao", tinNhan.DaXoaVao);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteTinNhan(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM TinNhan WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
