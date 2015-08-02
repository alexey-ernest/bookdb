using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDb.DAL
{
    public interface IBookRepository
    {
        Task<List<Book>> GetByPublishedDateAsync();

        Task<List<Book>> GetByTitleAsync();

        Task<Book> GetAsync(int id);

        Task<Book> AddAsync(Book book);

        Task<Book> UpdateAsync(Book book);

        Task<Book> DeleteAsync(int id);
    }
}