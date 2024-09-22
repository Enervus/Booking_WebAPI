using Booking.Domain.Interfaces;

namespace Booking.Domain.Entity
{
    public class Role : IEntityId<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
