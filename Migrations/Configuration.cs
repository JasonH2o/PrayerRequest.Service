namespace PrayerRequest.Service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PrayerRequest.Service.Models;    
using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<PrayerRequest.Service.DataContext.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PrayerRequest.Service.DataContext.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var prayerRequests = new List<PrayerRequestDetail>()
            {
                new PrayerRequestDetail()
                {
                    Id = 1,
                    Date = DateTime.Today,
                    IsCurrent = false,
                    Name = "Jason Liu",
                    Request = "Hope this Application work"
                }
            };

            prayerRequests.ForEach(x => context.PrayerRequests.Add(x));
            context.SaveChanges();
        }
    }
}
