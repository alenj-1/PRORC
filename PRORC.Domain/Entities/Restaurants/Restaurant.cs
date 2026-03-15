using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Entities.Restaurants
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CuisineType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<RestaurantAvailability> Availabilities { get; set; } = new();

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
