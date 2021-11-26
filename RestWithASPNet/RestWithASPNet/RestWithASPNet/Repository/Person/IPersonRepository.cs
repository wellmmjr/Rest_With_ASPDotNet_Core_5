using System.Collections.Generic;

namespace RestWithASPNet.Repository.Person
{
    public interface IPersonRepository : IRepository<Model.Person>
    {
       Model.Person ActiveUser(long id);

        List<Model.Person> FindByName(string firstName, string secondName);
    }
}
