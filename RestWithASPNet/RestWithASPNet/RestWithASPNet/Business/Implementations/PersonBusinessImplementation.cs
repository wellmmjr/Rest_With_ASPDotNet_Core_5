using RestWithASPNet.Data.Converter.Implementation;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Hypermedia.Utils;
using RestWithASPNet.Model;
using RestWithASPNet.Repository;
using RestWithASPNet.Repository.Person;
using System.Collections.Generic;

namespace RestWithASPNet.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IPersonRepository _repository;

        private readonly PersonConverter _converter;
        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO ActiveUser(long id)
        {
            var person = _repository.ActiveUser(id);

            return _converter.Parse(person);
        }

        public PersonVO Create(PersonVO personVO)
        {
            var person = _converter.Parse(personVO);
            _repository.Create(person);

            return _converter.Parse(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }


        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public List<PersonVO> FindByName(string firstName, string secondName)
        {
            return _converter.Parse(_repository.FindByName(firstName, secondName));
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int currentPage)
        {
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = currentPage > 0 ? (currentPage - 1) * size : 0;

            string query = @"SELECT * FROM person P WHERE 1 = 1";

            string countQuery = @"SELECT COUNT(*) FROM person P WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(name))
            {
                query += $" AND P.first_name LIKE '{name}%'";
                countQuery += $" AND P.first_name LIKE '{name}%'";
                
            }
            query += $" ORDER BY P.first_name {sort} LIMIT {size} OFFSET {offset}";

            var people = _repository.FindWithPagedSearch(query);
            int totalResluts = _repository.GetCount(countQuery);

            return new PagedSearchVO<PersonVO> { 
                CurrentPage = currentPage,
                List = _converter.Parse(people),
                PageSize = size,
                SortDirection = sort,
                TotalResults = totalResluts
            };
        }

        public PersonVO Update(PersonVO personVO)
        {
            var person = _converter.Parse(personVO);
            _repository.Update(person);

            return _converter.Parse(person);
        }
    }
}
