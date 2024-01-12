using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface TinNhanIconRepository
    {
        List<TinNhanIcon> GetTinNhanIconList();
        TinNhanIcon GetTinNhanIconById(int id);
        void AddTinNhanIcon(TinNhanIcon tinNhanIcon);
        void UpdateTinNhanIcon(TinNhanIcon tinNhanIcon);
        void DeleteTinNhanIcon(int id);

    }
}
