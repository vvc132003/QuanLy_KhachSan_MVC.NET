﻿using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface PhongRepository
    {
        List<Phong> GetAllPhong();
        List<Phong>  GetAllPhongTrangThai();
        List<Phong> GetAllPhongIDTang(int idtang);
        Phong GetPhongID(int id);
        void CapNhatPhong(Phong phong);
    }
}
