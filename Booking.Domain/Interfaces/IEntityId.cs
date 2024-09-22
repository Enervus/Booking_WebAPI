using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces
{
    public interface IEntityId<T> where T : struct //struct,чтобы не было строк; T, чтобы для таблиц различного объема были разной длины id 
    {
        public T Id { get; set; }
    }
}
