using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Reservations
{
    public class ReservationRepository : BaseRepository<Reservation, int>, IReservationRepository
    {
        public ReservationRepository(PRORCContext context) : base(context)
        {
        }
    }
}
