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
                                phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
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
                string query = "INSERT INTO MaGiamGia (magiamgia, mota, phantramgiamgia, solansudungtoida, solandasudung, ngaybatdau, ngayketthuc, idquydinhgiamgia)" +
                " VALUES (@MaGiamGia, @MoTa, @PhanTramGiamGia, @solansudungtoida, @solandasudung, @NgayBatDau, @NgayKetThuc, @IDQuyDinhGiamGia)";
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
                                phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
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
                                phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
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
                                phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]),
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
        private static int GenerateRandomInvoiceNumber()
        {
            // Generate a random invoice number between 1 and 1000 (adjust as needed)
            Random random = new Random();
            return random.Next(1, 1001);
        }
        private string BootstrapStyles()
        {
            return @"
        /* Copy and paste the minified Bootstrap CSS here or link to the CDN */
        <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>
    ";
        }
        public void GuiEmailThanhToan(KhachHang khachHang, float tienphong, float phantramgiamgia, int sophong, DateTime ngayden, List<ThueSanPham> listthueSanPham, float tongtien)
        {
            try
            {
                string fromEmail = "vvc132003@gmail.com";
                string password = "bzcaumaekzuvwlia";
                string toEmail = khachHang.email;
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Thanh toán thành công";
                int invoiceNumber = GenerateRandomInvoiceNumber();
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine(@"
            <div style='margin: 0; padding: 0;'>
                <div style='width: 50%; margin: auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                    <div style='display: flex;'>
                        <div style='width: 50%; margin-bottom: 20px;'>
                            <img style='width: 100%;' src='https://png.pngtree.com/png-clipart/20191123/original/pngtree-hotel-building-vector-illustration-with-simple-design-hotel-icon-png-image_5194507.jpg' alt='Hotel Logo'>
                        </div>
                        <div style='width: 50%; margin-top: 20px;'>
                            <h3>Khách sạn hotel</h3>
                            <p>Địa chỉ: TP.Huế</p>
                            <p>Điện thoại: 0373449865</p>
                            <p>Website: chinh.com</p>
                        </div>
                    </div>
                    <h4 style='margin-top: 20px; text-align: center;'>Hoá đơn thanh toán</h4>
                    <div style='display: flex;'>
                        <div style='width: 50%;'>
                            " + DateTime.Now + @"
                        </div>
                        <div style='width: 50%;'>
                            Mã số hoá đơn: " + invoiceNumber + @"
                        </div>
                    </div>
                    <hr>
                    <div style='display: flex;'>
                        <div style='width: 50%; margin-top: 20px;'>
                            <p>Tên: " + khachHang.hovaten + @"</p>
                            <p>Địa chỉ: " + khachHang.tinh + @"</p>
                            <p>Điện thoại: " + khachHang.sodienthoai + @"</p>
                            <p>Mã phòng: " + sophong + @"</p>
                            <p>Mã phòng: " + tienphong + @"</p>
                        </div>
                        <div style='width: 50%; margin-top: 20px; text-align: end;'>
                            <p>Ngày đến: " + ngayden + @"</p>
                            <p>Ngày đi: " + DateTime.Now + @"</p>
                            <p>Số đêm: 2</p>
                            <p>Thanh toán: Đã thanh toán</p>
                        </div>
                    </div>
                    <hr>
                    <div style='display: flex;'>
                        <div style='width: 25%;'></div>
                        <div style='width: 75%; margin-top: 20px;'>
                            <table style='width: 100%;'>
                                <thead>
                                    <tr>
                                        <th style='text-align: left;'>Tên sản phẩm</th>
                                        <th style='text-align: center;'>Số lượng</th>
                                        <th style='text-align: right;'>Thành tiền</th>
                                    </tr>
                                </thead>
                                <tbody>
                            ");
                foreach (var thueSanPham in listthueSanPham)
                {
                    bodyBuilder.AppendLine($@"
                                    <tr>
                                        <td>{thueSanPham.tensanpham}</td>
                                        <td style='text-align: center;'>{thueSanPham.soluong}</td>
                                        <td style='text-align: right;'>{thueSanPham.thanhtien} VND</td>
                                    </tr>
                                ");
                }
                bodyBuilder.AppendLine(@"
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <p style='text-align: end;' >Mã giảm giá: " + phantramgiamgia + "%" + @"</p>
                    <p style='text-align: end;' >Tổng tiền: " + tongtien + @"</p>
                </div>
            </div>
        ");
                message.Body = bodyBuilder.ToString();
                message.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi: " + ex.Message);
            }
        }

    }
}