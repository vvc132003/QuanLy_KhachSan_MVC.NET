using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Service
{
    public class LichSuThanhToanService : LichSuThanhToanRepository
    {
        public void ThemLichSuThanhToan(LichSuThanhToan lichSuThanhToan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string sql = "INSERT INTO LichSuThanhToan (ngaythanhtoan, sotienthanhtoan, hinhthucthanhtoan, trangthai,phantramgiamgia, iddatphong, idnhanvien) " +
                    "VALUES (@ngaythanhtoan, @sotienthanhtoan, @hinhthucthanhtoan, @trangthai,@phantramgiamgia, @iddatphong, @idnhanvien);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ngaythanhtoan", lichSuThanhToan.ngaythanhtoan);
                    command.Parameters.AddWithValue("@sotienthanhtoan", lichSuThanhToan.sotienthanhtoan);
                    command.Parameters.AddWithValue("@hinhthucthanhtoan", lichSuThanhToan.hinhthucthanhtoan);
                    command.Parameters.AddWithValue("@trangthai", lichSuThanhToan.trangthai);
                    command.Parameters.AddWithValue("@phantramgiamgia", lichSuThanhToan.phantramgiamgia);
                    command.Parameters.AddWithValue("@iddatphong", lichSuThanhToan.iddatphong);
                    command.Parameters.AddWithValue("@idnhanvien", lichSuThanhToan.idnhanvien);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<LichSuThanhToan> GetAllLichSuThanhToanDescNgayThanhToan()
        {
            List<LichSuThanhToan> lichSuThanhToans = new List<LichSuThanhToan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM LichSuThanhToan ORDER BY ngaythanhtoan DESC";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan();
                            lichSuThanhToan.ngaythanhtoan = reader.GetDateTime("ngaythanhtoan");
                            lichSuThanhToan.sotienthanhtoan = Convert.ToSingle(reader["sotienthanhtoan"]);
                            lichSuThanhToan.hinhthucthanhtoan = reader.GetString("hinhthucthanhtoan");
                            lichSuThanhToan.trangthai = reader.GetString("trangthai");
                            lichSuThanhToan.phantramgiamgia = Convert.ToSingle(reader["phantramgiamgia"]);
                            lichSuThanhToan.iddatphong = reader.GetInt32("iddatphong");
                            lichSuThanhToan.idnhanvien = reader.GetInt32("idnhanvien");

                            lichSuThanhToans.Add(lichSuThanhToan);
                        }
                    }
                }
            }

            return lichSuThanhToans;
        }
        public List<LichSuThanhToan> GetLichSuThanhToan()
        {
            List<LichSuThanhToan> lichSuThanhToanList = new List<LichSuThanhToan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT MONTH(ngaythanhtoan) as month, SUM(sotienthanhtoan) as total " +
                               "FROM LichSuThanhToan " +
                               "GROUP BY MONTH(ngaythanhtoan) " +
                               "ORDER BY MONTH(ngaythanhtoan)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = Convert.ToInt32(reader["month"]);
                            float total = Convert.ToSingle(reader["total"]);
                            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan
                            {
                                ngaythanhtoan = new DateTime(1, month, 1),
                                sotienthanhtoan = total,
                            };

                            lichSuThanhToanList.Add(lichSuThanhToan);
                        }
                    }
                }
            }

            return lichSuThanhToanList;
        }

        public List<LichSuThanhToan> GetLichSuThanhToanYear(int year)
        {
            List<LichSuThanhToan> lichSuThanhToanList = new List<LichSuThanhToan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT MONTH(ngaythanhtoan) as month, SUM(sotienthanhtoan) as total " +
                               "FROM LichSuThanhToan " +
                               "WHERE YEAR(ngaythanhtoan) = @Year " +  
                               "GROUP BY MONTH(ngaythanhtoan) " +
                               "ORDER BY MONTH(ngaythanhtoan)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", year); // Add parameter for year

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = Convert.ToInt32(reader["month"]);
                            float total = Convert.ToSingle(reader["total"]);
                            // Create a DateTime object for the month and year (assuming day as 1)
                            DateTime date = new DateTime(year, month, 1);

                            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan
                            {
                                ngaythanhtoan = date,
                                sotienthanhtoan = total,
                            };

                            lichSuThanhToanList.Add(lichSuThanhToan);
                        }
                    }
                }
            }

            return lichSuThanhToanList;
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
                            <p>Website: sofwaremini.com</p>
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
                            <p>Gia phòng: " + string.Format("{0:N0} VNĐ", tienphong) + @"</p>
                        </div>
                        <div style='width: 50%; margin-top: 20px; text-align: end;'>
                            <p>Ngày đến: " + ngayden + @"</p>
                            <p>Ngày đi: " + DateTime.Now + @"</p>
                            <p>Số đêm: " + (DateTime.Now.Day - ngayden.Day) + @"</p>
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
                                        <td style='text-align: right;'>{string.Format("{0:N0} VNĐ", thueSanPham.thanhtien)}</td>
                                    </tr>
                                ");
                }
                bodyBuilder.AppendLine(@"
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <p style='text-align: end;' >Mã giảm giá: " + phantramgiamgia + "%" + @"</p>
                    <p style='text-align: end;' >Tổng tiền: " + string.Format("{0:N0} VNĐ", tongtien) + @"</p>
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