using ketnoicsdllan1;
using Model.Models;
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
    public class XacMinhService
    {
        public void Create(XacMinh xacMinh)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO XacMinh (email, maso, thoigianhethan) VALUES (@Email, @MaSo, @ThoiGianHetHan)", connection);
                cmd.Parameters.AddWithValue("@Email", xacMinh.email);
                cmd.Parameters.AddWithValue("@MaSo", xacMinh.maso);
                cmd.Parameters.AddWithValue("@ThoiGianHetHan", xacMinh.thoigianhethan);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Read
        public XacMinh GetById(int id)
        {
            XacMinh xacMinh = null;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM XacMinh WHERE id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        xacMinh = new XacMinh
                        {
                            id = (int)reader["id"],
                            email = reader["email"].ToString(),
                            maso = reader["maso"].ToString(),
                            thoigianhethan = (DateTime)reader["thoigianhethan"]
                        };
                    }
                }
            }
            return xacMinh;
        }
        public XacMinh GetBymaso(string maso)
        {
            XacMinh xacMinh = null;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM XacMinh WHERE maso = @maso", connection);
                cmd.Parameters.AddWithValue("@maso", maso);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        xacMinh = new XacMinh
                        {
                            id = (int)reader["id"],
                            email = reader["email"].ToString(),
                            maso = reader["maso"].ToString(),
                            thoigianhethan = (DateTime)reader["thoigianhethan"]
                        };
                    }
                }
            }
            return xacMinh;
        }

        // Update
        public void Update(XacMinh xacMinh)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("UPDATE XacMinh SET email = @Email, maso = @MaSo, thoigianhethan = @ThoiGianHetHan WHERE id = @Id", connection);
                cmd.Parameters.AddWithValue("@Email", xacMinh.email);
                cmd.Parameters.AddWithValue("@MaSo", xacMinh.maso);
                cmd.Parameters.AddWithValue("@ThoiGianHetHan", xacMinh.thoigianhethan);
                cmd.Parameters.AddWithValue("@Id", xacMinh.id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete
        public void Delete(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM XacMinh WHERE id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // GetAll
        public List<XacMinh> GetAll()
        {
            List<XacMinh> xacMinhs = new List<XacMinh>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM XacMinh", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        XacMinh xacMinh = new XacMinh
                        {
                            id = (int)reader["id"],
                            email = reader["email"].ToString(),
                            maso = reader["maso"].ToString(),
                            thoigianhethan = (DateTime)reader["thoigianhethan"]
                        };
                        xacMinhs.Add(xacMinh);
                    }
                }
            }
            return xacMinhs;
        }
        public void GuiEmailXacMinh(XacMinh xacMinh)
        {
            try
            {
                string fromEmail = "vvc132003@gmail.com";
                string password = "bzcaumaekzuvwlia";
                string toEmail = xacMinh.email;
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Mã xác minh đăng ký";
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"Mã xác minh của bạn là: {xacMinh.maso}");
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
