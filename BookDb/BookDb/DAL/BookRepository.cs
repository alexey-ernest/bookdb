using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookDb.Exceptions;

namespace BookDb.DAL
{
    public class BookRepository : IBookRepository
    {
        private readonly IBookDatabase _db;

        public BookRepository(IBookDatabase db)
        {
            _db = db;
        }

        public Task<List<Book>> GetByPublishedDateAsync()
        {
            return _db.GetBooksByPublishedDateAsync();
        }

        public Task<List<Book>> GetByTitleAsync()
        {
            return _db.GetBooksByTitleAsync();
        }

        public Task<Book> GetAsync(int id)
        {
            return _db.GetBookAsync(id);
        }

        public Task<Book> AddAsync(Book book)
        {
            if (book.Authors == null || !book.Authors.Any())
            {
                throw new BadRequestException("At least 1 author required.");
            }

            return _db.AddBookAsync(book);
        }

        public Task<Book> UpdateAsync(Book book)
        {
            if (book.Authors == null || !book.Authors.Any())
            {
                throw new BadRequestException("At least 1 author required.");
            }

            return _db.UpdateBookAsync(book);
        }

        public Task<Book> DeleteAsync(int id)
        {
            return _db.DeleteBookAsync(id);
        }
    }
}