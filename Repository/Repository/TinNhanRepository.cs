using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface TinNhanRepository
    {
        List<TinNhan> GetTinNhanList();
        void AddTinNhan(TinNhan tinNhan);
        void UpdateTinNhan(TinNhan tinNhan);
        void DeleteTinNhan(int id);
    }
}
