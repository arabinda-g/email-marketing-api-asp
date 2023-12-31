﻿using EmailMarketingWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmailMarketingWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Campaign>? Campaigns { get; set; }
        public virtual DbSet<EmailQueue>? EmailQueue { get; set; }
        public virtual DbSet<Recipient>? Recipients { get; set; }
        public virtual DbSet<EmailTracking>? EmailTracking { get; set; }
    }

}
