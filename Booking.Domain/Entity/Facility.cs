using Booking.Domain.Enum;
using Booking.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entity
{
    public class Facility:IEntityId<long>,IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public FacilityType FacilityType { get; set; }
        public string Address { get; set; }
        public DateTime EndOfLease { get; set; }
        public PostStatus PostStatus { get; set; }
        public int Cost { get; set; }
        public double Coefficient { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public string ImgName { get; set; }
        public string ImgPath { get; set; }
        public List<Order> Orders { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public long CreatedBy { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        public long? UpdatedBy { get ; set ; }
    }
}
