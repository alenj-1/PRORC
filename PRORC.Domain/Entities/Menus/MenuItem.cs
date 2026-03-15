using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Entities.Menus
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public Menu? Menu { get; set; }
    }
}
