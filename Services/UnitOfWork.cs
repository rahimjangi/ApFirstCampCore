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

    public UnitOfWork()
    {
        PersonService =  new PersonService();
        CountriesService = new CountriesService();
    }
}
