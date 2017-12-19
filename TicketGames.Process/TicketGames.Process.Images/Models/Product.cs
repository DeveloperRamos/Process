using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGames.Process.Images.Models
{
    public class Product
    {
        public long Id { get; set; }
        public List<Image> Images { get; set; }
        public Product()
        {
            this.Images = new List<Image>();
        }
    }
}
