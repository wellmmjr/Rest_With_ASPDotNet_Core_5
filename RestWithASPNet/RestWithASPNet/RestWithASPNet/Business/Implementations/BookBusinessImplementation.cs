using RestWithASPNet.Data.Converter.Implementation;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Model;
using RestWithASPNet.Repository;
using System.Collections.Generic;

namespace RestWithASPNet.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {

        private readonly IRepository<Book> _repository;

        private readonly BookConverter _converter;

        public BookBusinessImplementation(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }

        public BookVO Create(BookVO bookVO)
        {
            var book = _converter.Parse(bookVO);
            _repository.Create(book);

            return _converter.Parse(book);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public List<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public BookVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public BookVO Update(BookVO bookVO)
        {
            var book = _converter.Parse(bookVO);
            _repository.Update(book);

            return _converter.Parse(book);
        }
    }
}
