using APIAspNetCore5.Model;
using APIAspNetCore5.Repository.Generic;
using System.Collections.Generic;

namespace APIAspNetCore5.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        List<Person> FindByName(string fristName, string lastName);
    }
}