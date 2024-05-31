using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface GiamGiaNgayLeRepository
    {
        List<GiamGiaNgayLe> GetAllGiamGiaNgayLe();
        void ThemGiamGiaNgayLe(GiamGiaNgayLe giamGiaNgayLe);
        void EditGiamGiaNgayLe(GiamGiaNgayLe giamGiaNgayLe);
        GiamGiaNgayLe GetGiamGiaNgayLeById(int id);
        GiamGiaNgayLe GetGiamGiaNgayLeByNgayLe(DateTime ngayle);
        void DeleteGiamGiaNgayLe(int id);

    }
}
