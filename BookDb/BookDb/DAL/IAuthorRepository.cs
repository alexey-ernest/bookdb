using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDb.DAL
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetByNameAsync();

        Task<Author> GetAsync(int id);

        Task<Author> AddAsync(Author author);

        Task<Author> UpdateAsync(Author author);

        Task<Author> DeleteAsync(int id);
    }
}