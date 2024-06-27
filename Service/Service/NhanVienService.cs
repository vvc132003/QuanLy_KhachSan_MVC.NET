using ketnoicsdllan1;
using Model.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics;

namespace Service
{
    public class NhanVienService : NhanVienRepository
    {
        public NhanVien GetNhanVienDangNhap(string matkhau, string taikhoan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string CheckThongTinDangNhap = "SELECT NhanVien.*, ChucVu.*, KhachSan.* FROM NhanVien LEFT JOIN ChucVu ON NhanVien.idchucvu = ChucVu.id  LEFT JOIN KhachSan ON NhanVien.idkhachsan = KhachSan.id  WHERE NhanVien.taikhoan = @taikhoan  AND NhanVien.matkhau = @matkhau and (ChucVu.tenchucvu = N'Quản lý' or ChucVu.tenchucvu = N'Nhân viên') ";
                using (SqlCommand command = new SqlCommand(CheckThongTinDangNhap, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    command.Parameters.AddWithValue("@matkhau", matkhau);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                tenchucvu = reader["tenchucvu"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                                idkhachsan = (int)reader["idkhachsan"],
                            };
                            return nhanVien;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public NhanVien CheckThongTinDangNhap(string matkhau, string taikhoan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string CheckThongTinDangNhap = "SELECT NhanVien.*, ChucVu.*, KhachSan.* FROM NhanVien LEFT JOIN ChucVu ON NhanVien.idchucvu = ChucVu.id  LEFT JOIN KhachSan ON NhanVien.idkhachsan = KhachSan.id  WHERE NhanVien.taikhoan = @taikhoan  AND NhanVien.matkhau = @matkhau";
                using (SqlCommand command = new SqlCommand(CheckThongTinDangNhap, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    command.Parameters.AddWithValue("@matkhau", matkhau);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                tenchucvu = reader["tenchucvu"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                                idkhachsan = (int)reader["idkhachsan"],
                            };
                            return nhanVien;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public NhanVien CheckThongTinDangNhaps(string taikhoan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string CheckThongTinDangNhap = "SELECT NhanVien.*, ChucVu.*, KhachSan.* FROM NhanVien LEFT JOIN ChucVu ON NhanVien.idchucvu = ChucVu.id  LEFT JOIN KhachSan ON NhanVien.idkhachsan = KhachSan.id  WHERE NhanVien.taikhoan = @taikhoan";
                using (SqlCommand command = new SqlCommand(CheckThongTinDangNhap, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                tenchucvu = reader["tenchucvu"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                                idkhachsan = (int)reader["idkhachsan"],
                            };
                            return nhanVien;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public void CapNhatNhanVien(NhanVien nhanVien)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string updateQuery = @"UPDATE NhanVien
                                   SET hovaten = @hovaten,
                                       sodienthoai = @sodienthoai,
                                       tinh = @tinh,
                                       huyen = @huyen,
                                       phuong = @phuong,
                                       taikhoan = @taikhoan,
                                       matkhau = @matkhau,
                                       trangthai = @trangthai,
                                       solanvipham = @solanvipham,
                                       cccd = @cccd,
                                       gioitinh = @gioitinh,
                                       ngaysinh = @ngaysinh,
                                       image = @image
                                       WHERE id = @id";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@hovaten", nhanVien.hovaten);
                    command.Parameters.AddWithValue("@sodienthoai", nhanVien.sodienthoai);
                    command.Parameters.AddWithValue("@tinh", nhanVien.tinh);
                    command.Parameters.AddWithValue("@huyen", nhanVien.huyen);
                    command.Parameters.AddWithValue("@phuong", nhanVien.phuong);
                    command.Parameters.AddWithValue("@taikhoan", nhanVien.taikhoan);
                    command.Parameters.AddWithValue("@matkhau", nhanVien.matkhau);
                    command.Parameters.AddWithValue("@trangthai", nhanVien.trangthai);
                    command.Parameters.AddWithValue("@solanvipham", nhanVien.solanvipham);
                    command.Parameters.AddWithValue("@cccd", nhanVien.cccd);
                    command.Parameters.AddWithValue("@gioitinh", nhanVien.gioitinh);
                    command.Parameters.AddWithValue("@ngaysinh", nhanVien.ngaysinh);
                    command.Parameters.AddWithValue("@image", nhanVien.image);
                    command.Parameters.AddWithValue("@idchucvu", nhanVien.idchucvu);

                    command.Parameters.AddWithValue("@id", nhanVien.id);
                    command.ExecuteNonQuery();
                }
            }
        }



        public List<NhanVien> GetAllNhanVien()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<NhanVien> nhanViens = new List<NhanVien>();
                connection.Open();
                string selectQuery = "SELECT * FROM NhanVien";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                                idkhachsan = (int)reader["idkhachsan"]

                            };
                            nhanViens.Add(nhanVien);
                        }
                    }
                }
                return nhanViens;
            }
        }

        public NhanVien GetNhanVienID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from NhanVien where id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                            };
                            return nhanVien;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public NhanVien NhanVienTonTai(string taikhoan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from NhanVien where taikhoan = @taikhoan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taikhoan", taikhoan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                            };
                            return nhanVien;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public int ThemNhanVien(NhanVien nhanVien)
        {
            int idnhanvienthemvao = 0;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string insertQuery = "INSERT INTO NhanVien (hovaten, sodienthoai, tinh, huyen, phuong, taikhoan," +
                     " matkhau, trangthai, solanvipham, cccd, gioitinh, ngaysinh, image, idchucvu, idkhachsan) " +
                     "VALUES (@hovaten, @sodienthoai, @tinh, @huyen, @phuong, @taikhoan, @matkhau, " +
                     "@trangthai, @solanvipham, @cccd, @gioitinh, @ngaysinh, @image, @idchucvu, @idkhachsan) " +
                     "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@hovaten", nhanVien.hovaten);
                    command.Parameters.AddWithValue("@sodienthoai", nhanVien.sodienthoai);
                    command.Parameters.AddWithValue("@tinh", nhanVien.tinh);
                    command.Parameters.AddWithValue("@huyen", nhanVien.huyen);
                    command.Parameters.AddWithValue("@phuong", nhanVien.phuong);
                    command.Parameters.AddWithValue("@taikhoan", nhanVien.taikhoan);
                    command.Parameters.AddWithValue("@matkhau", nhanVien.matkhau);
                    command.Parameters.AddWithValue("@trangthai", nhanVien.trangthai);
                    command.Parameters.AddWithValue("@solanvipham", nhanVien.solanvipham);
                    command.Parameters.AddWithValue("@cccd", nhanVien.cccd);
                    command.Parameters.AddWithValue("@gioitinh", nhanVien.gioitinh);
                    command.Parameters.AddWithValue("@ngaysinh", nhanVien.ngaysinh);
                    command.Parameters.AddWithValue("@image", nhanVien.image);
                    command.Parameters.AddWithValue("@idchucvu", nhanVien.idchucvu);
                    command.Parameters.AddWithValue("@idkhachsan", nhanVien.idkhachsan);
                    idnhanvienthemvao = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return idnhanvienthemvao;
        }

        public void XoaNhanVien(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "DELETE FROM NhanVien WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<NhanVien> GetallNhanVientheoidbophan(int idbophan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<NhanVien> nhanViens = new List<NhanVien>();
                connection.Open();
                string sql = "SELECT * FROM NhanVien where idbophan = @idbophan";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idbophan", idbophan);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                sodienthoai = reader["sodienthoai"].ToString(),
                                tinh = reader["tinh"].ToString(),
                                huyen = reader["huyen"].ToString(),
                                phuong = reader["phuong"].ToString(),
                                taikhoan = reader["taikhoan"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                solanvipham = (int)reader["solanvipham"],
                                cccd = reader["cccd"].ToString(),
                                gioitinh = reader["gioitinh"].ToString(),
                                image = reader["image"].ToString(),
                                idchucvu = (int)reader["idchucvu"],

                            };
                            nhanViens.Add(nhanVien);
                        }
                    }
                }
                return nhanViens;
            }
        }

        public void Xuatexcel()
        {
            List<NhanVien> nhanViens = GetAllNhanVien();
            string filePath = "nhanViens.xlsx";
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Danh sách nhân viên");
            IRow headerRow = worksheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Id");
            headerRow.CreateCell(1).SetCellValue("Tên nhân viên");
            headerRow.CreateCell(2).SetCellValue("Số điện thoại");
            headerRow.CreateCell(3).SetCellValue("Tỉnh - Huyện - Xã");
            headerRow.CreateCell(4).SetCellValue("Số lần vi phạm");
            headerRow.CreateCell(5).SetCellValue("CCCD");
            headerRow.CreateCell(6).SetCellValue("Giới tính");
            headerRow.CreateCell(7).SetCellValue("Ngày sinh");
            headerRow.CreateCell(8).SetCellValue("Chức vụ");

            headerRow.CreateCell(11).SetCellValue("Trạng thái");
            int rowIndex = 1;
            foreach (var nhanVien in nhanViens)
            {
                IRow dataRow = worksheet.CreateRow(rowIndex);
                dataRow.CreateCell(0).SetCellValue(nhanVien.id);
                dataRow.CreateCell(1).SetCellValue(nhanVien.hovaten);
                dataRow.CreateCell(2).SetCellValue(nhanVien.sodienthoai);
                dataRow.CreateCell(3).SetCellValue(nhanVien.tinh + "-" + nhanVien.huyen + "-" + nhanVien.phuong);
                dataRow.CreateCell(4).SetCellValue(nhanVien.solanvipham);
                dataRow.CreateCell(5).SetCellValue(nhanVien.cccd);
                dataRow.CreateCell(6).SetCellValue(nhanVien.gioitinh);
                dataRow.CreateCell(7).SetCellValue(nhanVien.ngaysinh.ToString("yyyy-MM-dd"));
                dataRow.CreateCell(8).SetCellValue(nhanVien.idchucvu);

                dataRow.CreateCell(11).SetCellValue(nhanVien.trangthai);
                rowIndex++;
            }
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(file);
            }
            GuiEmail(filePath);
        }
        public void GuiEmail(string filePath)
        {
            string emailnguoigui = "vvc132003@gmail.com";
            string matkhau = "bzcaumaekzuvwlia";
            string emailnguoinhan = "vvc132003@gmail.com";
            using (MailMessage message = new MailMessage(emailnguoigui, emailnguoinhan))
            {
                message.Subject = "Gửi file excel nhân viên thành công";
                Attachment attachment = new Attachment(filePath);
                message.Attachments.Add(attachment);

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailnguoigui, matkhau);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }
                attachment.Dispose();
                /*                File.Delete(filePath);
                */
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filePath;
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }
    }
}