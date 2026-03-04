using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Restaurants
{
    public class RestaurantRepository : BaseRepository<Restaurant, int>, IRestaurantRepository
    {
        public RestaurantRepository(PRORCContext context) : base(context)
        {
        }
    }
}
