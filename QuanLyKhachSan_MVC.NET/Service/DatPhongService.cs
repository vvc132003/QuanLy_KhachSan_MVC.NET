using DocumentFormat.OpenXml.Office2010.Excel;
using ketnoicsdllan1;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Repository;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace QuanLyKhachSan_MVC.NET.Service
{
    public class DatPhongService : DatPhongRepository
    {
        public List<DatPhong> GetAllDatPhong()
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<DatPhong> datPhongs = new List<DatPhong>();
                connection.Open();
                string sql = "SELECT * FROM DatPhong where trangthai = N'đã đặt online' ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong()
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idkhachhang = (int)reader["idkhachhang"],
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"],
                                tiendatcoc = Convert.ToSingle(reader["tiendatcoc"]),
                                idloaidatphong = (int)reader["idloaidatphong"],
                                idthoigian = (int)reader["idthoigian"],
                            };
                            datPhongs.Add(datPhong);
                        }
                    }
                }
                return datPhongs;
            }
        }

        public List<DatPhong> GetAllDatPhongByID(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                List<DatPhong> datPhongs = new List<DatPhong>();
                connection.Open();
                string sql = "SELECT * FROM DatPhong left join KhachHang on datphong.idkhachhang = khachhang.id where idphong = @idphong and DatPhong.trangthai = N'đã đặt' ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idphong", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong()
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idkhachhang = (int)reader["idkhachhang"],
                                hovaten = reader["hovaten"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"],
                                tiendatcoc = Convert.ToSingle(reader["tiendatcoc"]),
                                idloaidatphong = (int)reader["idloaidatphong"],
                                idthoigian = (int)reader["idthoigian"],
                            };
                            datPhongs.Add(datPhong);
                        }
                    }
                }
                return datPhongs;
            }
        }

        public DatPhong GetDatPhongByIDTrangThai(int idphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from DatPhong where idphong = @idphong and trangthai= N'đã đặt' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idphong", idphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idloaidatphong = (int)reader["idloaidatphong"],
                                idthoigian = (int)reader["idthoigian"],
                                idkhachhang = (int)reader["idkhachhang"],
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"],
                                tiendatcoc = Convert.ToSingle(reader["tiendatcoc"]),
                            };
                            return datPhong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public DatPhong GetDatPhongByIDTrangThaiOnline(int idphong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "select * from DatPhong left join KhachHang on datphong.idkhachhang = khachhang.id where DatPhong.idphong = @idphong and DatPhong.trangthai= N'đã đặt online' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idphong", idphong);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DatPhong datPhong = new DatPhong
                            {
                                id = (int)reader["id"],
                                idphong = (int)reader["idphong"],
                                idkhachhang = (int)reader["idkhachhang"],
                                hovaten = reader["hovaten"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                hinhthucthue = reader["hinhthucthue"].ToString(),
                                ngaydat = (DateTime)reader["ngaydat"],
                                ngaydukientra = (DateTime)reader["ngaydukientra"],
                                tiendatcoc = Convert.ToSingle(reader["tiendatcoc"]),
                                idloaidatphong = (int)reader["idloaidatphong"],
                                idthoigian = (int)reader["idthoigian"],
                                cccd = reader["cccd"].ToString(),
                            };
                            return datPhong;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public int GetDatPhongCountByKhachHangId(int idkhachhang)
        {
            int count = 0;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM DatPhong WHERE idkhachhang = @idkhachhang";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idkhachhang", idkhachhang);
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }


        public int ThemDatPhong(DatPhong datPhong)
        {
            int idDatPhongThemVao = 0;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "INSERT INTO DatPhong (ngaydat, ngaydukientra, tiendatcoc, trangthai,hinhthucthue, idloaidatphong, idkhachhang, idphong,idthoigian) " +
                               "VALUES (@ngaydat, @ngaydukientra, @tiendatcoc, @trangthai,@hinhthucthue, @idloaidatphong, @idkhachhang, @idphong,@idthoigian)" +
                               "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ngaydat", datPhong.ngaydat);
                command.Parameters.AddWithValue("@ngaydukientra", datPhong.ngaydukientra);
                command.Parameters.AddWithValue("@tiendatcoc", datPhong.tiendatcoc);
                command.Parameters.AddWithValue("@trangthai", datPhong.trangthai);
                command.Parameters.AddWithValue("@hinhthucthue", datPhong.hinhthucthue);
                command.Parameters.AddWithValue("@idloaidatphong", datPhong.idloaidatphong);
                command.Parameters.AddWithValue("@idkhachhang", datPhong.idkhachhang);
                command.Parameters.AddWithValue("@idphong", datPhong.idphong);
                command.Parameters.AddWithValue("@idthoigian", datPhong.idthoigian);
                idDatPhongThemVao = Convert.ToInt32(command.ExecuteScalar());
            }
            return idDatPhongThemVao;
        }

        public void UpdateDatPhong(DatPhong datPhong)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string updateQuery = "UPDATE DatPhong SET ngaydat = @ngaydat, ngaydukientra = @ngaydukientra, " +
                                   "tiendatcoc = @tiendatcoc, trangthai = @trangthai, hinhthucthue = @hinhthucthue, idloaidatphong=@idloaidatphong," +
                                   " idkhachhang=@idkhachhang, idphong=@idphong, idthoigian = @idthoigian " +
                                   " WHERE id = @id";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@ngaydat", datPhong.ngaydat);
                command.Parameters.AddWithValue("@ngaydukientra", datPhong.ngaydukientra);
                command.Parameters.AddWithValue("@tiendatcoc", datPhong.tiendatcoc);
                command.Parameters.AddWithValue("@trangthai", datPhong.trangthai);
                command.Parameters.AddWithValue("@hinhthucthue", datPhong.hinhthucthue);
                command.Parameters.AddWithValue("@idloaidatphong", datPhong.idloaidatphong);
                command.Parameters.AddWithValue("@idkhachhang", datPhong.idkhachhang);
                command.Parameters.AddWithValue("@idphong", datPhong.idphong);
                command.Parameters.AddWithValue("@idthoigian", datPhong.idthoigian);
                command.Parameters.AddWithValue("@id", datPhong.id);
                command.ExecuteNonQuery();
            }
        }
        public void GuiEmail(KhachHang khachHang, DatPhong datPhong, Phong phong, ThoiGian thoiGian)
        {
            try
            {
                string fromEmail = "vvc132003@gmail.com";
                string password = "bzcaumaekzuvwlia";
                string toEmail = khachHang.email;
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Bạn đã đặt phòng thành công";
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"Tên sinh viên: {khachHang.hovaten}");
                bodyBuilder.AppendLine($"Số phòng: {phong.sophong}");
                bodyBuilder.AppendLine($"Ngày dự kiến trả: {datPhong.ngaydukientra}");
                bodyBuilder.AppendLine($"Ngày thuê: {datPhong.ngaydat}");
                bodyBuilder.AppendLine($"Thời gian check_In và Out của khách sạn là: {thoiGian.thoigiannhanphong - thoiGian.thoigianra}");
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
