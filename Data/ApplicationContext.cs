using KriptoshtampTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace KriptoshtampTestTask.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "MockupData.json";
            string mockupJson = File.ReadAllText(path);
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(mockupJson);

            foreach(var i in users)
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Surname = i.Surname,
                        MiddleName = i.MiddleName,
                        DateOfBirth = i.DateOfBirth,
                        Country= i.Country,
                        City= i.City,
                        Gender= i.Gender,
                    });
            }

        }

        public DbSet<User> Users { get; set; }
    }
}
