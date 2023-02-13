using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");
            string countriesJson=System.IO.File.ReadAllText("countries.json");
            string personsJson = System.IO.File.ReadAllText("persons.json");

            List<Country> countriesList= System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            List<Person> personList= System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Country country  in countriesList) 
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            foreach (Person person in personList)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

        }
    }
}
