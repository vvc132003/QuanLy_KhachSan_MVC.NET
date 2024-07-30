using DocumentFormat.OpenXml.Office.Word;
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
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"]),
                            daXem = reader["daXem"].ToString(),
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
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"]),
                            daXem = reader["daXem"].ToString(),

                        };

                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }

        public async Task<TinNhan> GetTinNhanByIdAsync(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                await connection.OpenAsync();

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
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"]),
                            daXem = reader["daXem"].ToString(),

                        };
                    }
                }
            }

            return null; // If no record is found
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
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"]),
                            daXem = reader["daXem"].ToString(),

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
                            DaXoaVao = Convert.ToDateTime(reader["DaXoaVao"]),
                            daXem = reader["daXem"].ToString(),

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

                string query = "INSERT INTO TinNhan (CuocHoiThoaiId, NhanVienGuiId, LoaiTinNhan, NoiDung, DuocTaoVao, DaXoaVao, daXem) " +
                               "VALUES (@CuocHoiThoaiId, @NhanVienGuiId, @LoaiTinNhan, @NoiDung, @DuocTaoVao, @DaXoaVao, @daXem)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CuocHoiThoaiId", tinNhan.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienGuiId", tinNhan.NhanVienGuiId);
                command.Parameters.AddWithValue("@LoaiTinNhan", tinNhan.LoaiTinNhan);
                command.Parameters.AddWithValue("@NoiDung", tinNhan.NoiDung);
                command.Parameters.AddWithValue("@DuocTaoVao", tinNhan.DuocTaoVao);
                command.Parameters.AddWithValue("@DaXoaVao", tinNhan.DaXoaVao);
                command.Parameters.AddWithValue("@daXem", tinNhan.daXem);


                command.ExecuteNonQuery();
            }
        }
        public async Task AddTinNhanBuh(int cuochoithoaiid, int nhanvienguiid, string noidung, string loaitinnhan, string daXem)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO TinNhan (CuocHoiThoaiId, NhanVienGuiId, LoaiTinNhan, NoiDung, DuocTaoVao, DaXoaVao, daXem) " +
                               "VALUES (@CuocHoiThoaiId, @NhanVienGuiId, @LoaiTinNhan, @NoiDung, GETDATE(), GETDATE(), @daXem)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CuocHoiThoaiId", cuochoithoaiid);
                command.Parameters.AddWithValue("@NhanVienGuiId", nhanvienguiid);
                command.Parameters.AddWithValue("@LoaiTinNhan", loaitinnhan);
                command.Parameters.AddWithValue("@NoiDung", noidung);
                command.Parameters.AddWithValue("@daXem", daXem);


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
                               "NoiDung = @NoiDung, DuocTaoVao = @DuocTaoVao, DaXoaVao = @DaXoaVao, daXem=@daXem " +
                               "WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", tinNhan.Id);
                command.Parameters.AddWithValue("@CuocHoiThoaiId", tinNhan.CuocHoiThoaiId);
                command.Parameters.AddWithValue("@NhanVienGuiId", tinNhan.NhanVienGuiId);
                command.Parameters.AddWithValue("@LoaiTinNhan", tinNhan.LoaiTinNhan);
                command.Parameters.AddWithValue("@NoiDung", tinNhan.NoiDung);
                command.Parameters.AddWithValue("@DuocTaoVao", tinNhan.DuocTaoVao);
                command.Parameters.AddWithValue("@DaXoaVao", tinNhan.DaXoaVao);
                command.Parameters.AddWithValue("@daXem", tinNhan.daXem);


                command.ExecuteNonQuery();
            }
        }

        public async Task UpdateTinNhanAsync(TinNhan tinNhan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                await connection.OpenAsync();

                string query = "UPDATE TinNhan SET CuocHoiThoaiId = @CuocHoiThoaiId, " +
                               "NhanVienGuiId = @NhanVienGuiId, LoaiTinNhan = @LoaiTinNhan, " +
                               "NoiDung = @NoiDung, DuocTaoVao = @DuocTaoVao, DaXoaVao = @DaXoaVao, daXem = @daXem " +
                               "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", tinNhan.Id);
                    command.Parameters.AddWithValue("@CuocHoiThoaiId", tinNhan.CuocHoiThoaiId);
                    command.Parameters.AddWithValue("@NhanVienGuiId", tinNhan.NhanVienGuiId);
                    command.Parameters.AddWithValue("@LoaiTinNhan", tinNhan.LoaiTinNhan);
                    command.Parameters.AddWithValue("@NoiDung", tinNhan.NoiDung);
                    command.Parameters.AddWithValue("@DuocTaoVao", tinNhan.DuocTaoVao);
                    command.Parameters.AddWithValue("@DaXoaVao", tinNhan.DaXoaVao);
                    command.Parameters.AddWithValue("@daXem", tinNhan.daXem);

                    await command.ExecuteNonQueryAsync();
                }
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
