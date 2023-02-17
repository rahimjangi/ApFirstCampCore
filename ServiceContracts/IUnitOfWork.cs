using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts;

public interface IUnitOfWork
{
    public IPersonsService PersonsService { get; set; }
    public ICountriesService CountriesService { get; set; }
}
