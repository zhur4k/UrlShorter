using UrlShorter.Models;

namespace UrlShorter.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.UrlEntry.Any())
                {
                    context.UrlEntry.AddRange(new List<UrlEntry>()
                    {
                        new UrlEntry()
                        {
                            LongUrl = "https://github.com/teddysmithdev/RunGroop/blob/master/RunGroopWebApp/Data/Seed.cs",
                            ShortUrl = "RunGroop/blob/master/RunGroopWebApp/Data/Seed.cs",
                            ClickCount = 2,
                            CreatedDate = DateTime.Now.AddDays(1),
                         },
                        new UrlEntry()
                        {
                            LongUrl = "https://github.com/teddysmithdev/RunGroop",
                            ShortUrl = "RunGroop/blob",
                            ClickCount = 5,
                            CreatedDate = DateTime.Now.AddDays(3),
                         }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
