using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;
using AutoRep.Models;

namespace AutoRep.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AutoRep.Models.Work> Work { get; set; }
        public DbSet<AutoRep.Models.WorkType> WorkType { get; set; }
        public DbSet<AutoRep.Models.UserRequest> Request { get; set; }
    }
}
