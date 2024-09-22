using Booking.Domain.Interfaces;

namespace Booking.Domain.Entity
{
    public class Order : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public User User { get; set; }
        public long TotalCost { get; set; }
        public long UserId { get; set; }
        public List<Facility> Facilities { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
