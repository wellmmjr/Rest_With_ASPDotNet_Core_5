using RestWithASPNet.Data.VO;
using RestWithASPNet.Model;
using System.Collections.Generic;

namespace RestWithASPNet.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO personVO);

        PersonVO FindById(long id);

        List<PersonVO> FindAll();

        PersonVO Update(PersonVO personVO);

        void Delete(long id);
    }
}
