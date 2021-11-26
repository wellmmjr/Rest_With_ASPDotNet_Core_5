using RestWithASPNet.Data.VO;
using System.Collections.Generic;

namespace RestWithASPNet.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);

        BookVO Update(BookVO book);

        List<BookVO> FindAll();

        BookVO FindById(long id);

        void Delete(long id);
    }
}
