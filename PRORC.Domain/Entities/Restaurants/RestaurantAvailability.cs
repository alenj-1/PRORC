using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Entities.Restaurants
{
    public class RestaurantAvailability
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AvailableTables { get; set; }
        
        public Restaurant? Restaurant { get; set; }
    }
}
