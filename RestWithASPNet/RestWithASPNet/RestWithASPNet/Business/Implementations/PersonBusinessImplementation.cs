using RestWithASPNet.Data.Converter.Implementation;
using RestWithASPNet.Data.VO;
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

        public PersonVO Update(PersonVO personVO)
        {
            var person = _converter.Parse(personVO);
            _repository.Update(person);

            return _converter.Parse(person);
        }
    }
}
