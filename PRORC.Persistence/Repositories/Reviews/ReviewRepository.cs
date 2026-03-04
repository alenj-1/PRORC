using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Reviews
{
    public class ReviewRepository : BaseRepository<Review, int>, IReviewRepository
    {
        public ReviewRepository(PRORCContext context) : base(context)
        {
        }
    }
}
