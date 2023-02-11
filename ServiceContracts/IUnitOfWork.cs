using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts;

public interface IUnitOfWork
{
    public IPersonService PersonService { get; set; }
    public ICountriesService CountriesService { get; set; }
}
