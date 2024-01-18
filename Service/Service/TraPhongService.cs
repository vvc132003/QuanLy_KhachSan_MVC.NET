﻿using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace  Service
{
    public class TraPhongService : TraPhongRepository
    {
        public void ThemTraPhong(TraPhong traPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO TraPhong (ngaytra, idnhanvien, iddatphong) VALUES (@ngaytra, @idnhanvien, @iddatphong);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ngaytra", traPhong.ngaytra);
                    command.Parameters.AddWithValue("@idnhanvien", traPhong.idnhanvien);
                    command.Parameters.AddWithValue("@iddatphong", traPhong.iddatphong);
                    command.ExecuteNonQuery();
                }
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
