using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDb.DAL
{
    public interface IBookDatabase
    {
        Task<List<Book>> GetBooksByPublishedDateAsync();

        Task<List<Book>> GetBooksByTitleAsync();

        Task<Book> GetBookAsync(int id);

        Task<Book> AddBookAsync(Book book);

        Task<Book> UpdateBookAsync(Book book);

        Task<Book> DeleteBookAsync(int id);


        Task<List<Author>> GetAuthorsByNameAsync();

        Task<Author> GetAuthorAsync(int id);

        Task<Author> AddAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(Author author);

        Task<Author> DeleteAuthorAsync(int id);
    }
}