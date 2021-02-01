using APIAspNetCore5.Data.Converters;
using APIAspNetCore5.Data.VO;
using APIAspNetCore5.Hypermedia.Utils;
using APIAspNetCore5.Model;
using APIAspNetCore5.Repository;
using APIAspNetCore5.Repository.Generic;
using System.Collections.Generic;

namespace APIAspNetCore5.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {

        private IPersonRepository _repository;

        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Create(personEntity);
            return _converter.Parse(personEntity);
        }

        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public List<PersonVO> FindAll()
        {
            return _converter.ParseList(_repository.FindAll());
        }
        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.ParseList(_repository.FindByName(firstName, lastName));
        }

        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public PagedSearchDTO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var offset = page > 0 ? (page - 1) * pageSize : 0;
            var sort = (!string.IsNullOrEmpty(name)) && !name.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 1 : pageSize;

            string query = @"select * from Persons p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) query = query + $" and p.firstName like '%{name}%'";

            query += $" order by p.firstName {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from Persons p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) countQuery += $" and p.firstName like '%{name}%'";

            var persons = _repository.FindWithPagedSearch(query);

            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchDTO<PersonVO>
            {
                CurrentPage = offset,
                List = _converter.ParseList(persons),
                PageSize = pageSize,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public bool Exists(long id)
        {
            return _repository.Exists(id);
        }
    }
}