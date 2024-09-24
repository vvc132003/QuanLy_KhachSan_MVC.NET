using ketnoicsdllan1;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Service.Service
{
    public class QRCodeRequestService
    {
        // Thêm một bản ghi mới vào bảng
        public void AddQRCodeRequest(QRCodeRequest qrCodeRequest)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO QRCode (taikhoan, tentaikhoan, machinhanh, ngaytao) VALUES (@taikhoan, @tentaikhoan, @machinhanh, @ngaytao)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", qrCodeRequest.taikhoan);
                    command.Parameters.AddWithValue("@tentaikhoan", qrCodeRequest.tentaikhoan);
                    command.Parameters.AddWithValue("@machinhanh", qrCodeRequest.machinhanh);
                    command.Parameters.AddWithValue("@ngaytao", qrCodeRequest.ngaytao);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Lấy tất cả bản ghi
        public List<QRCodeRequest> GetAllQRCodeRequests()
        {
            var qrCodeRequests = new List<QRCodeRequest>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM QRCode";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var qrCodeRequest = new QRCodeRequest
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                taikhoan = reader.GetString(reader.GetOrdinal("taikhoan")),
                                tentaikhoan = reader.GetString(reader.GetOrdinal("tentaikhoan")),
                                machinhanh = reader.GetString(reader.GetOrdinal("machinhanh")),
                                ngaytao = reader.GetDateTime(reader.GetOrdinal("ngaytao"))
                            };
                            qrCodeRequests.Add(qrCodeRequest);
                        }
                    }
                }
            }

            return qrCodeRequests;
        }

        // Lấy bản ghi theo Id
        public async Task<QRCodeRequest> GetQRCodeByIdAsync(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM QRCode WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return new QRCodeRequest
                        {
                            id = (int)reader["id"],
                            taikhoan = reader["taikhoan"].ToString(),
                            tentaikhoan = reader["tentaikhoan"].ToString(),
                            machinhanh = reader["machinhanh"].ToString(),
                            ngaytao = (DateTime)reader["ngaytao"]
                        };
                    }
                    return null;
                }
            }
        }

        // Cập nhật bản ghi
        public void UpdateQRCodeRequest(QRCodeRequest qrCodeRequest)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "UPDATE QRCod SET taikhoan = @taikhoan, tentaikhoan = @tentaikhoan, machinhanh = @machinhanh, ngaytao = @ngaytao WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", qrCodeRequest.id);
                    command.Parameters.AddWithValue("@taikhoan", qrCodeRequest.taikhoan);
                    command.Parameters.AddWithValue("@tentaikhoan", qrCodeRequest.tentaikhoan);
                    command.Parameters.AddWithValue("@machinhanh", qrCodeRequest.machinhanh);
                    command.Parameters.AddWithValue("@ngaytao", qrCodeRequest.ngaytao);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Xóa bản ghi
        public void DeleteQRCodeRequest(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM QRCode WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
