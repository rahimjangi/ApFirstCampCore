using Entities;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class UnitOfWork : IUnitOfWork
{
    public IPersonsService PersonsService { get; set; }
    public ICountriesService CountriesService { get; set; }
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        CountriesService = new CountriesService(new CountriesRepository(_db));
        PersonsService = new PersonsService(new PersonsRepository(_db));
    }
}
