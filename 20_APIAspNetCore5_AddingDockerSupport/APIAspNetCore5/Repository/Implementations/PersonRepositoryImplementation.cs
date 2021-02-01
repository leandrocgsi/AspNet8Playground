using APIAspNetCore5.Model;
using APIAspNetCore5.Model.Context;
using APIAspNetCore5.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAspNetCore5.Repository.Implementations
{
    public class PersonRepositoryImplementation : GenericRepository<Person>, IPersonRepository
    {

        public PersonRepositoryImplementation(MySQLContext context) : base(context) { }

        public List<Person> FindByName(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName)).ToList();
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.LastName.Contains(lastName)).ToList();
            }
            else if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(firstName)).ToList();
            }
            return _context.Persons.ToList();
        }
    }
}
