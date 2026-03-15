using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.DTOs.Restaurants
{
    public class CreateRestaurantRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CuisineType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
