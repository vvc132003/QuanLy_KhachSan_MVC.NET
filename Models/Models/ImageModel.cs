using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
    }

}
