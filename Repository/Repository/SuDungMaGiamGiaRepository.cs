using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface SuDungMaGiamGiaRepository
    {
        void ThemSuDungMaGiamGia(SuDungMaGiamGia suDungMaGiamGia);
        void XoaSuDungMaGiamGia(int id);
        void CapNhatSuDungMaGiamGia(SuDungMaGiamGia suDungMaGiamGia);
        List<SuDungMaGiamGia> GetAllSuDungMaGiamGia();
        SuDungMaGiamGia GetSuDungMaGiamGiaById(int id);
    }
}
