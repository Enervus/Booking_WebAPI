using Booking.Domain.Entity;
using Booking.Domain.Interfaces.Databases;
using Booking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context; 

        public UnitOfWork(ApplicationDbContext context, IBaseRepository<User> users, IBaseRepository<Role> roles, IBaseRepository<UserRole> userRoles, IBaseRepository<Order> orders, IBaseRepository<Facility> facilties)
        {
            _context = context;
            Users = users;
            Roles = roles;
            UserRoles = userRoles;
            Orders = orders;
            Facilities = facilties;
        }

        public IBaseRepository<User> Users { get; set; }
        public IBaseRepository<Role> Roles { get; set; }
        public IBaseRepository<UserRole> UserRoles { get; set; }
        public IBaseRepository<Order> Orders { get; set; }
        public IBaseRepository<Facility> Facilities { get; set; }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }

     /*   public void Dispose()
        {
            GC.SuppressFinalize(this);
        }*/
    }
}
