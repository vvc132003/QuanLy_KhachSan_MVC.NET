using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface NgayLeRepository
    {
        void ThemNgayLe(NgayLe ngayLe);
        void CapNhatNgayLe(NgayLe ngayLe);
        void XoaNgayLe(int id);
        List<NgayLe> GetAllNgayLes();
        NgayLe GetNgayLeById(int id);
    }
}