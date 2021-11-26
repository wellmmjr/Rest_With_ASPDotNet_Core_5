using RestWithASPNet.Model;
using System.Collections.Generic;

namespace RestWithASPNet.Business
{
    public interface IBookBusiness
    {
        Book Create(Book book);

        Book Update(Book book);

        List<Book> FindAll();

        Book FindById(long id);

        void Delete(long id);
    }
}
