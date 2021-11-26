using RestWithASPNet.Model.Context;
using RestWithASPNet.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNet.Repository.Person
{
    public class PersonRepository : GenericRepository<Model.Person>, IPersonRepository
    {
        public PersonRepository(MySQLContext context) : base (context) { }

        public Model.Person ActiveUser(long id)
        {
            if(!_context.People.Any(p => p.Id.Equals(id))) return null;

            var user = _context.People.SingleOrDefault(p => p.Id.Equals(id));

            if(user != null)
            {
                user.Enabled = !user.Enabled;

                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return user;
        }

        public List<Model.Person> FindByName(string firstName, string secondName)
        {
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(secondName))
            {
                return _context.People.Where(p => p.FirstName.Contains(firstName) 
                && p.SecondNamed.Contains(secondName)).ToList();
            }else if (!string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(secondName))
            {
                return _context.People.Where(p => p.FirstName.Contains(firstName)).ToList();
            }else if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(secondName))
            {
                return _context.People.Where(p => p.SecondNamed.Contains(secondName)).ToList();
            }

            return null;
        }
    }
}
