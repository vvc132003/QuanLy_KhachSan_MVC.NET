using ketnoicsdllan1;
using Model.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class MaGiamGiaService : MaGiamGiaRepository
    {
        public List<MaGiamGia> GetAllMaGiamGia()
        {
            List<MaGiamGia> magiamgiaList = new List<MaGiamGia>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM MaGiamGia";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MaGiamGia magiamgia = new MaGiamGia
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                magiamgia = reader.GetString(reader.GetOrdinal("magiamgia")),
                                mota = reader.GetString(reader.GetOrdinal("mota")),
                                phantramgiamgia = (float)reader["phantramgiamgia"],
                                ngaybatdau = reader.GetDateTime(reader.GetOrdinal("ngaybatdau")),
                                ngayketthuc = reader.GetDateTime(reader.GetOrdinal("ngayketthuc")),
                                idquydinhgiamgia = reader.GetInt32(reader.GetOrdinal("idquydinhgiamgia")),
                                solansudungtoida = reader.GetInt32(reader.GetOrdinal("solansudungtoida")),
                                solandasudung = reader.GetInt32(reader.GetOrdinal("solandasudung")),
                            };

                            magiamgiaList.Add(magiamgia);
                        }
                    }
                }
            }
            return magiamgiaList;
        }

        public void CapNhatMaGiamGia(MaGiamGia maGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "UPDATE MaGiamGia SET magiamgia = @MaGiamGia, mota = @MoTa, phantramgiamgia = @PhanTramGiamGia,solansudungtoida=@solansudungtoida,solandasudung=@solandasudung, ngaybatdau = @NgayBatDau, ngayketthuc = @NgayKetThuc, idquydinhgiamgia = @IDQuyDinhGiamGia WHERE id = @ID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaGiamGia", maGiamGia.magiamgia);
                command.Parameters.AddWithValue("@MoTa", maGiamGia.mota);
                command.Parameters.AddWithValue("@PhanTramGiamGia", maGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@NgayBatDau", maGiamGia.ngaybatdau);
                command.Parameters.AddWithValue("@NgayKetThuc", maGiamGia.ngayketthuc);
                command.Parameters.AddWithValue("@IDQuyDinhGiamGia", maGiamGia.idquydinhgiamgia);
                command.Parameters.AddWithValue("@solansudungtoida", maGiamGia.solansudungtoida);
                command.Parameters.AddWithValue("@solandasudung", maGiamGia.solandasudung);
                command.Parameters.AddWithValue("@ID", maGiamGia.id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ThemMaGiamGia(MaGiamGia maGiamGia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO MaGiamGia (magiamgia, mota, phantramgiamgia,solansudungtoida,solandasudung, ngaybatdau, ngayketthuc, idquydinhgiamgia)" +
                    " VALUES (@MaGiamGia, @MoTa, @PhanTramGiamGia,@solansudungtoida,@solandasudung @NgayBatDau, @NgayKetThuc, @IDQuyDinhGiamGia)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaGiamGia", maGiamGia.magiamgia);
                command.Parameters.AddWithValue("@MoTa", maGiamGia.mota);
                command.Parameters.AddWithValue("@PhanTramGiamGia", maGiamGia.phantramgiamgia);
                command.Parameters.AddWithValue("@NgayBatDau", maGiamGia.ngaybatdau);
                command.Parameters.AddWithValue("@NgayKetThuc", maGiamGia.ngayketthuc);
                command.Parameters.AddWithValue("@IDQuyDinhGiamGia", maGiamGia.idquydinhgiamgia);
                command.Parameters.AddWithValue("@solansudungtoida", maGiamGia.solansudungtoida);
                command.Parameters.AddWithValue("@solandasudung", maGiamGia.solandasudung);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void XoaMaGiamGia(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM MaGiamGia WHERE id = @ID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public MaGiamGia GetMaGiamGiaById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM MaGiamGia WHERE id = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MaGiamGia maGiamGia = new MaGiamGia
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                magiamgia = reader.GetString(reader.GetOrdinal("magiamgia")),
                                mota = reader.GetString(reader.GetOrdinal("mota")),
                                phantramgiamgia = (float)reader["phantramgiamgia"],
                                ngaybatdau = reader.GetDateTime(reader.GetOrdinal("ngaybatdau")),
                                ngayketthuc = reader.GetDateTime(reader.GetOrdinal("ngayketthuc")),
                                idquydinhgiamgia = reader.GetInt32(reader.GetOrdinal("idquydinhgiamgia")),
                                solansudungtoida = reader.GetInt32(reader.GetOrdinal("solansudungtoida")),
                                solandasudung = reader.GetInt32(reader.GetOrdinal("solandasudung")),
                            };
                            return maGiamGia;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public MaGiamGia GetMaGiamGiaByIdQuyDinhGiamGia(int idquydinhgiamgia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM MaGiamGia WHERE idquydinhgiamgia = @idquydinhgiamgia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idquydinhgiamgia", idquydinhgiamgia);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MaGiamGia maGiamGia = new MaGiamGia
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                magiamgia = reader.GetString(reader.GetOrdinal("magiamgia")),
                                mota = reader.GetString(reader.GetOrdinal("mota")),
                                phantramgiamgia = (float)reader["phantramgiamgia"],
                                ngaybatdau = reader.GetDateTime(reader.GetOrdinal("ngaybatdau")),
                                ngayketthuc = reader.GetDateTime(reader.GetOrdinal("ngayketthuc")),
                                idquydinhgiamgia = reader.GetInt32(reader.GetOrdinal("idquydinhgiamgia")),
                                solansudungtoida = reader.GetInt32(reader.GetOrdinal("solansudungtoida")),
                                solandasudung = reader.GetInt32(reader.GetOrdinal("solandasudung")),
                            };
                            return maGiamGia;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public MaGiamGia GetMaGiamGiaByMaGiamGia(string magiamgia)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM MaGiamGia WHERE magiamgia = @magiamgia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@magiamgia", magiamgia);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MaGiamGia maGiamGia = new MaGiamGia
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                magiamgia = reader.GetString(reader.GetOrdinal("magiamgia")),
                                mota = reader.GetString(reader.GetOrdinal("mota")),
                                phantramgiamgia = (float)reader["phantramgiamgia"],
                                ngaybatdau = reader.GetDateTime(reader.GetOrdinal("ngaybatdau")),
                                ngayketthuc = reader.GetDateTime(reader.GetOrdinal("ngayketthuc")),
                                idquydinhgiamgia = reader.GetInt32(reader.GetOrdinal("idquydinhgiamgia")),
                                solansudungtoida = reader.GetInt32(reader.GetOrdinal("solansudungtoida")),
                                solandasudung = reader.GetInt32(reader.GetOrdinal("solandasudung")),
                            };
                            return maGiamGia;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public void GuiEmail(KhachHang khachHang, string magiamgia)
        {
            try
            {
                string fromEmail = "vvc132003@gmail.com";
                string password = "bzcaumaekzuvwlia";
                string toEmail = khachHang.email;
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Bạn đã đặt phòng thành công";
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"Mã giảm giá: {magiamgia}");
                message.Body = bodyBuilder.ToString();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi: " + ex.Message);
            }
        }

    }
}
