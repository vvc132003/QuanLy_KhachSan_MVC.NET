using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IconRepository
    {
        List<Icon> GetIconList();
        void AddIcon(Icon icon);
        void UpdateIcon(Icon icon);
        void DeleteIcon(int id);
    }
}
