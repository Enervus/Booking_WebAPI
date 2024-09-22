using Booking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Booking.DAL.Interceptors
{
    public class DateInterceptor:SaveChangesInterceptor
    {

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, 
            CancellationToken cancellationToken =  new CancellationToken())
        {
            var dbContext = eventData.Context;//получили DbContext
            if(dbContext == null ) 
            {
                return base.SavingChangesAsync(eventData, result,cancellationToken);
            }

            var entries = dbContext.ChangeTracker.Entries<IAuditable>()
                .Where(x=>x.State == EntityState.Added || x.State == EntityState.Modified)
                .ToList();

            foreach(var entry in entries )
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property(x=>x.CreatedAt).CurrentValue = DateTime.UtcNow;//Interceptor будет сам менять даты создания и даты изменения, поэтому и создавался IAuditable
                }
                if(entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SavingChangesAsync(eventData, result,cancellationToken);
        }
    }
}
