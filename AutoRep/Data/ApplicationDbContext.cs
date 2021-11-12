using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;
using AutoRep.Models;

namespace AutoRep.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AutoRep.Models.User> User { get; set; }
        public DbSet<AutoRep.Models.Work> Work { get; set; }
        public DbSet<AutoRep.Models.WorkType> WorkType { get; set; }
    }
}
