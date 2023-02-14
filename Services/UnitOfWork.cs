using Entities;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class UnitOfWork : IUnitOfWork
{
    public IPersonService PersonService { get; set; }
    public ICountriesService CountriesService { get; set; }

    public UnitOfWork(ApplicationDbContext _db)
    {
        PersonService =  new PersonService(_db);
        CountriesService = new CountriesService(_db);
    }
}
