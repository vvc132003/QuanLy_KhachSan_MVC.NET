﻿using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface TangRepository
    {
        List<Tang> GetAllTang();
        void ThemTang(Tang tang);
        void CapNhatTang(Tang tang);
        void XoaTang(int id);
        Tang GetTangID(int id);
    }
}
