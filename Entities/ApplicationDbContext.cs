using Microsoft.Data.SqlClient;
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
        public List<Person> sp_getAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_addPerson(Person person)
        {
        //    @PersonName NVARCHAR(50),
        //@Email NVARCHAR(50),
        //@DateOfBirth DATE,
        //@Gender NVARCHAR(6),
        //@CountryId UNIQUEIDENTIFIER,
        //@Address NVARCHAR(150),
        //@ReceiveNewsLetters BIT
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@PersonId",person.PersonId),
                new SqlParameter("@PersonName",person.PersonName),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender",person.Gender),
                new SqlParameter("@Address",person.Address),
                new SqlParameter("@CountryId",person.CountryId),
                
            };
            SqlParameter ReceiveNewsLetters = new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters);
            parameters.Add(ReceiveNewsLetters);
            int rows = Database.ExecuteSqlRaw("EXECUTE [dbo].[SP_INSERTPERSON] @PersonId,@PersonName, @Email, @DateOfBirth, @Gender, @CountryId,@Address, @ReceiveNewsLetters", parameters);
            return rows;
        }
    }
}
