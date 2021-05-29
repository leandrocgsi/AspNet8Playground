using DomainModel.Model;
using System.Collections.Generic;

namespace DataAccessMySqlProvider.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string secondName);
    }
}
