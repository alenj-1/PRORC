using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.DTOs.Reservations
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public ReservationStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}