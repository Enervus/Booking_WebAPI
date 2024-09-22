using Booking.Domain.Entity;
using Booking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Databases
{
    public interface IUnitOfWork:IStateSaveChanges //: IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IBaseRepository<User> Users { get; set; }
        IBaseRepository<Role> Roles { get; set; }
        IBaseRepository<UserRole> UserRoles { get; set; }
        IBaseRepository<Order> Orders { get; set; }
        IBaseRepository<Facility> Facilities { get; set; }
    }
}
