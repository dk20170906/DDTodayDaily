using DDTodayDaily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DDTodayDaily.EF
{
    public class DailyDBContext : DbContext
    {
        public DailyDBContext() : base("name=ConnString")
        {
        }   
        public DbSet<Daily> DailyModel { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}