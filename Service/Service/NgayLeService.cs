﻿using ketnoicsdllan1;
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
    public class NgayLeService : NgayLeRepository
    {
        public void ThemNgayLe(NgayLe ngayLe)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "INSERT INTO NgayLe (ngayle, tenngayle, mota) VALUES (@NgayLe, @TenNgayLe, @MoTa)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NgayLe", ngayLe.ngayle);
                command.Parameters.AddWithValue("@TenNgayLe", ngayLe.tenngayle);
                command.Parameters.AddWithValue("@MoTa", ngayLe.mota);

                command.ExecuteNonQuery();
            }
        }
        public void CapNhatNgayLe(NgayLe ngayLe)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "UPDATE NgayLe SET ngayle = @NgayLe, tenngayle = @TenNgayLe, mota = @MoTa WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NgayLe", ngayLe.ngayle);
                command.Parameters.AddWithValue("@TenNgayLe", ngayLe.tenngayle);
                command.Parameters.AddWithValue("@MoTa", ngayLe.mota);
                command.Parameters.AddWithValue("@Id", ngayLe.id);

                command.ExecuteNonQuery();
            }
        }
        public void XoaNgayLe(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "DELETE FROM NgayLe WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
        public List<NgayLe> GetAllNgayLes()
        {
            List<NgayLe> ngayLes = new List<NgayLe>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NgayLe";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NgayLe ngayLe = MapNgayLeFromDataReader(reader);
                        ngayLes.Add(ngayLe);
                    }
                }
            }

            return ngayLes;
        }
        public NgayLe GetNgayLeById(int id)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = "SELECT * FROM NgayLe WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapNgayLeFromDataReader(reader);
                    }
                }
            }

            return null;
        }
        private NgayLe MapNgayLeFromDataReader(SqlDataReader reader)
        {
            return new NgayLe
            {
                id = (int)reader["id"],
                ngayle = (DateTime)reader["ngayle"],
                tenngayle = reader["tenngayle"].ToString(),
                mota = reader["mota"].ToString()
            };
        }
    }
}
