using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface CuocHoiThoaiRepository
    {
        List<CuocHoiThoai> GetCuocHoiThoaiList();

        void AddCuocHoiThoai(CuocHoiThoai cuocHoiThoai);

        void UpdateCuocHoiThoai(CuocHoiThoai cuocHoiThoai);


        void DeleteCuocHoiThoai(int id);

    }
}
