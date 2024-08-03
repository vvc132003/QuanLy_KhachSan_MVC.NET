using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class GopDonDatPhong
    {
        public int id { get; set; }

        public int iddatphongcu { get; set; }

        public int iddatphongmoi { get; set; }

        public float tienphong { get; set; }

    }
}
