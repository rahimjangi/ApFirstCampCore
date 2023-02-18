using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;

        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {
            var savedPerson=await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            return savedPerson.Entity;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personId)
        {
            var personFromDb=await _db.Persons.Include("Country").FirstOrDefaultAsync(p=> p.PersonId == personId);
            if(personFromDb!=null)
            {
                _db.Persons.Remove(personFromDb);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetyPersonByPersonID(Guid personID)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(p=>p.PersonId== personID);
        }

        public Person UpdatePerson(Person person)
        {
    
            Person? matchingPerson =  _db.Persons.FirstOrDefault(p=>p.PersonId==person.PersonId);

            if (matchingPerson == null)
            {

                return person;
            }
            _db.Persons.Remove(matchingPerson);

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
             _db.Persons.Add(person);
            int countUpdated =  _db.SaveChanges();

            return person;

        }
    }
}
