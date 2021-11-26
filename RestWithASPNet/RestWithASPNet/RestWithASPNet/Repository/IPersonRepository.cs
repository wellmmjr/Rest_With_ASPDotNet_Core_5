﻿using RestWithASPNet.Model;
using System.Collections.Generic;

namespace RestWithASPNet.Repository
{
    public interface IPersonRepository
    {
        Person Create(Person person);

        Person FindById(long id);

        List<Person> FindAll();

        Person Update(Person person);

        void Delete(long id);

        bool ExistsPerson(long id);
    }
}