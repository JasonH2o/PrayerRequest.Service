using System.Data.Entity;
using PrayerRequest.Service.Models;


namespace PrayerRequest.Service.DataContext
{
    public class DatabaseInitializer: DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            base.Seed(context);

            var prayerRequestForJason = new PrayerRequestDetail()
            {
                Id = 1,
                Name = "Jason",
                Date = System.DateTime.Now,
                IsCurrent = false,
                Request = "Hope to get a good house."
            };            

            context.PrayerRequests.Add(prayerRequestForJason);
            context.SaveChanges();
        }
    }
}