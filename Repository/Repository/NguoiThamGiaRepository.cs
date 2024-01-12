using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface NguoiThamGiaRepository
    {
        List<NguoiThamGia> GetNguoiThamGiaList();
        void AddNguoiThamGia(NguoiThamGia nguoiThamGia);
        void UpdateNguoiThamGia(NguoiThamGia nguoiThamGia);
        void DeleteNguoiThamGia(int id);
    }
}
