using RestWithASPNet.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RestWithASPNet.Services.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private volatile int count;

        public Person Create(Person person)
        {
            return person;
        }

        public void Delete(long id)
        {

        }

        public List<Person> FindAll()
        {
            List<Person> people = new List<Person>();

            for (int i = 0; i < 8; i++)
            {
                Person person = MockPerson(i);
                people.Add(person);
            }

            return people;
        }


        public Person FindById(long id)
        {
            return new Person
            {
                Id = IncrementAndGet(),
                FirstName = "Wellington",
                SecondNamed = "Mendes",
                Address = "rua dos bobos",
                Gender = "nono binary"
            };
        }

        public Person Update(Person person)
        {
            throw new System.NotImplementedException();
        }

        private Person MockPerson(int i)
        {
            return new Person
            {
                Id = IncrementAndGet(),
                FirstName = "Some Name "+i,
                SecondNamed = "Some 2 name "+i,
                Address = "rua dos bobos "+i,
                Gender = "nono binary "+i
            };
        }

        private long IncrementAndGet()
        {
            return Interlocked.Increment(ref count);
        }
    }
}
