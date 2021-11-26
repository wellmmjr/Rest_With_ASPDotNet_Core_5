using RestWithASPNet.Model;
using System.Collections.Generic;

namespace RestWithASPNet.Repository
{
    public interface IBookRepository
    {
        Book Create(Book book);

        Book Update(Book book);

        List<Book> FindAll();

        Book FindById(long id);

        void Delete(long id);

        bool ExistsBook(long id);
    }
}
