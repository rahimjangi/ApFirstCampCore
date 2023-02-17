﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);
        Task<List<Person>> GetAllPersons();
        Task<Person?> GetyPersonByPersonID(Guid personID);

        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool>> predicate);

        Task<bool> DeletePersonByPersonID(Guid personId);

        Task<Person> UpdatePerson(Person person);
    }
}
