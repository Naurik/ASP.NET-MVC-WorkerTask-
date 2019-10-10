using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskUser.Models
{
    public class ContextUser : DbContext
    {
        public DbSet<UserAccount> UsersAccount { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public ContextUser(DbContextOptions<ContextUser> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
