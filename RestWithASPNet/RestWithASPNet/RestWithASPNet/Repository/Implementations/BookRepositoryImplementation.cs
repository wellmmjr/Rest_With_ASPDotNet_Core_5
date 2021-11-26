using RestWithASPNet.Model;
using RestWithASPNet.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNet.Repository.Implementations
{
    public class BookRepositoryImplementation : IBookRepository
    {

        private MySQLContext _context;

        public BookRepositoryImplementation(MySQLContext context)
        {
            _context = context;
        }

        public Book Create(Book book)
        {
            try
            {
                _context.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return book;
        }

        public void Delete(long id)
        {
            var researchResult = _context.Book.SingleOrDefault(b => b.Id.Equals(id));

            if (researchResult != null)
            {
                try
                {
                    _context.Book.Remove(researchResult);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }


        public List<Book> FindAll()
        {
            return _context.Book.ToList();
        }

        public Book FindById(long id)
        {
            return _context.Book.SingleOrDefault(b => b.Id.Equals(id));
        }

        public Book Update(Book book)
        {
            if (!ExistsBook(book.Id)) return null;
            
            var existentBook = _context.Book.SingleOrDefault(b => b.Id.Equals(book.Id));

            if (existentBook != null)
            {
                try
                {
                    _context.Entry(existentBook).CurrentValues.SetValues(book);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            return book;
        }
        
        public bool ExistsBook(long id)
        {
            return _context.Book.Any(b => b.Id.Equals(id));
        }
    }
}
