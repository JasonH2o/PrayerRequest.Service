using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PrayerRequest.Service.Models;

namespace PrayerRequest.Service.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("H2OPrayerRequests") { }

        public DbSet<PrayerRequestDetail> PrayerRequests { get; set; }
    }
}