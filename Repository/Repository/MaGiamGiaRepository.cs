using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface MaGiamGiaRepository
    {
        void ThemMaGiamGia(MaGiamGia maGiamGia);
        void XoaMaGiamGia(int id);
        void CapNhatMaGiamGia(MaGiamGia maGiamGia);
        MaGiamGia GetMaGiamGiaById(int id);
        List<MaGiamGia> GetAllMaGiamGia();
    }
}
