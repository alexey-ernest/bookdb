using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDb.DAL
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IBookDatabase _db;

        public AuthorRepository(IBookDatabase db)
        {
            _db = db;
        }

        public Task<List<Author>> GetByNameAsync()
        {
            return _db.GetAuthorsByNameAsync();
        }

        public Task<Author> GetAsync(int id)
        {
            return _db.GetAuthorAsync(id);
        }

        public Task<Author> AddAsync(Author author)
        {
            return _db.AddAuthorAsync(author);
        }

        public Task<Author> UpdateAsync(Author author)
        {
            return _db.UpdateAuthorAsync(author);
        }

        public Task<Author> DeleteAsync(int id)
        {
            return _db.DeleteAuthorAsync(id);
        }
    }
}