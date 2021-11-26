using RestWithASPNet.Data.VO;
using RestWithASPNet.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNet.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO personVO);

        PersonVO FindById(long id);

        List<PersonVO> FindByName(string firstName, string secondName);

        List<PersonVO> FindAll();

        PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int currentPage);

        PersonVO Update(PersonVO personVO);

        PersonVO ActiveUser(long id);

        void Delete(long id);
    }
}
