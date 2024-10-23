using ketnoicsdllan1;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class TachDonService
    {
        public void InsertTachDon(TachDon tachDon)
        {
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "INSERT INTO TachDon (tensanpham, thanhtien, image, soluong, iddatphong, idsanpham, ghichu) " +
                               "VALUES (@tensanpham, @thanhtien, @image, @soluong, @iddatphong, @idsanpham, @ghichu)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tensanpham", tachDon.tensanpham);
                cmd.Parameters.AddWithValue("@thanhtien", tachDon.thanhtien);
                cmd.Parameters.AddWithValue("@image", tachDon.image);
                cmd.Parameters.AddWithValue("@soluong", tachDon.soluong);
                cmd.Parameters.AddWithValue("@iddatphong", tachDon.iddatphong);
                cmd.Parameters.AddWithValue("@idsanpham", tachDon.idsanpham);
                cmd.Parameters.AddWithValue("@ghichu", tachDon.ghichu);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<TachDon> GetAllTachDon()
        {
            List<TachDon> list = new List<TachDon>();
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM TachDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TachDon tachDon = new TachDon
                    {
                        id = Convert.ToInt32(reader["id"]),
                        tensanpham = reader["tensanpham"].ToString(),
                        thanhtien = Convert.ToSingle(reader["thanhtien"]), // Use Convert.ToSingle() here
                        image = reader["image"].ToString(),
                        soluong = Convert.ToInt32(reader["soluong"]),
                        iddatphong = Convert.ToInt32(reader["iddatphong"]),
                        idsanpham = Convert.ToInt32(reader["idsanpham"]),
                        ghichu = reader["ghichu"].ToString()
                    };
                    list.Add(tachDon);
                }
            }
            return list;
        }


        public List<TachDon> GetAllTachDonByDatPhongId(int iddatphong)
        {
            List<TachDon> list = new List<TachDon>();
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM TachDon WHERE iddatphong = @iddatphong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@iddatphong", iddatphong); 

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TachDon tachDon = new TachDon
                    {
                        id = Convert.ToInt32(reader["id"]),
                        tensanpham = reader["tensanpham"].ToString(),
                        thanhtien = Convert.ToSingle(reader["thanhtien"]), 
                        image = reader["image"].ToString(),
                        soluong = Convert.ToInt32(reader["soluong"]),
                        iddatphong = Convert.ToInt32(reader["iddatphong"]),
                        idsanpham = Convert.ToInt32(reader["idsanpham"]),
                        ghichu = reader["ghichu"].ToString()
                    };
                    list.Add(tachDon);
                }
            }
            return list;
        }


        public void UpdateTachDon(TachDon tachDon)
        {
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "UPDATE TachDon SET tensanpham = @tensanpham, thanhtien = @thanhtien, image = @image, soluong = @soluong, " +
                               "iddatphong = @iddatphong, idsanpham = @idsanpham, ghichu = @ghichu WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", tachDon.id);
                cmd.Parameters.AddWithValue("@tensanpham", tachDon.tensanpham);
                cmd.Parameters.AddWithValue("@thanhtien", tachDon.thanhtien);
                cmd.Parameters.AddWithValue("@image", tachDon.image);
                cmd.Parameters.AddWithValue("@soluong", tachDon.soluong);
                cmd.Parameters.AddWithValue("@iddatphong", tachDon.iddatphong );
                cmd.Parameters.AddWithValue("@idsanpham", tachDon.idsanpham );
                cmd.Parameters.AddWithValue("@ghichu",tachDon.ghichu );

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteTachDon(int id)
        {
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM TachDon WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteAllTachDon()
        {
            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                string query = "DELETE FROM TachDon"; 
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                cmd.ExecuteNonQuery(); 
            }
        }

        public TachDon GetTachDonByIdAndDatPhong(int idsanpham, int iddatphong)
        {
            string query = "SELECT * FROM TachDon WHERE idsanpham = @idsanpham AND iddatphong = @iddatphong";

            using (SqlConnection conn = DBUtils.GetDBConnection())
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@idsanpham", idsanpham);
                command.Parameters.AddWithValue("@iddatphong", iddatphong);

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TachDon
                        {
                            id = Convert.ToInt32(reader["id"]),
                            tensanpham = reader["tensanpham"].ToString(),
                            thanhtien = Convert.ToSingle(reader["thanhtien"]),
                            image = reader["image"].ToString(),
                            soluong = (int)reader["soluong"],
                            iddatphong = (int)reader["iddatphong"],
                            idsanpham = (int)reader["idsanpham"] ,
                            ghichu = reader["ghichu"].ToString()
                        };
                    }
                }
            }

            return null; // Return null if no record is found
        }


    }
}
