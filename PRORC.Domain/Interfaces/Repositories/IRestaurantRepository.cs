using PRORC.Domain.Entities.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant, int>
    {
    }
}
