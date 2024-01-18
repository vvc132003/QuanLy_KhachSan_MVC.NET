using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repository.Repository
{
    public interface ChinhSachGiaRepository
    {
        void ThemChinhSachGia(ChinhSachGia chinhSachGia);
        void CapNhatChinhSachGia(ChinhSachGia chinhSachGia);
        void XoaChinhSachGia(int id);
        List<ChinhSachGia> GetAllChinhSachGia();
        ChinhSachGia GetChinhSachGiaById(int id);
    }
}