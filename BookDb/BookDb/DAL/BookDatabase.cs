using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookDb.Exceptions;
using BookDb.Infrastructure.Threading;

namespace BookDb.DAL
{
    /// <summary>
    ///     In-memory book database with asynchronous API.
    /// </summary>
    public class BookDatabase : IBookDatabase
    {
        private readonly AsyncOneManyLock _lock = new AsyncOneManyLock();

        private int _authorIds = 1;
        private readonly Dictionary<int, Author> _authors = new Dictionary<int, Author>();
        private List<Author> _authorsByName = new List<Author>();

        private int _bookIds = 1;
        private readonly Dictionary<int, Book> _books = new Dictionary<int, Book>();        
        private readonly Dictionary<Author, List<Book>> _authorBooks = new Dictionary<Author, List<Book>>();
        private List<Book> _booksByPublishedDateDesc = new List<Book>();
        private List<Book> _booksByTitle = new List<Book>();

        public async Task<List<Book>> GetBooksByPublishedDateAsync()
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Shared);
                var result = _booksByPublishedDateDesc;

                // cloning books and their authors
                return Copy(result);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<Book>> GetBooksByTitleAsync()
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Shared);
                var result = _booksByTitle;

                // cloning books and their authors
                return Copy(result);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Book> GetBookAsync(int id)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Shared);
                var book = _books.ContainsKey(id) ? _books[id] : null;
                return Copy(book);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                book = Copy(book);

                // check consistency
                CheckBookRelations(book);

                // set id
                book.Id = _bookIds++;

                // load book relations
                LoadBookRelations(book);

                // add to storage
                _books.Add(book.Id, book);

                // update indexes
                _booksByTitle.Add(book);
                _booksByPublishedDateDesc.Add(book);
                UpdateBookIndexes();

                // sync author relations
                if (book.Authors != null)
                {
                    foreach (var author in book.Authors)
                    {
                        AddAuthorBookRelationship(book, author);
                    }
                }
            }
            finally
            {
                _lock.Release();
            }

            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                if (!_books.ContainsKey(book.Id))
                {
                    throw new NotFoundException(string.Format("Could not find book by id: {0}", book.Id));
                }

                // check consistency
                CheckBookRelations(book);

                book = Copy(book);
                var original = _books[book.Id];

                // updating author relations
                if (book.Authors != null)
                {
                    var deletedAuthors = original.Authors.Where(a => book.Authors.All(auth => auth.Id != a.Id));
                    foreach (var author in deletedAuthors)
                    {
                        DeleteAuthorBookRelationship(original, author);
                    }

                    var newAuthors = book.Authors.Where(a => original.Authors.All(auth => auth.Id != a.Id));
                    foreach (var author in newAuthors)
                    {
                        AddAuthorBookRelationship(original, author);
                    }
                }

                // updating book
                original.Title = book.Title;
                original.Authors = book.Authors;
                original.Pages = book.Pages;
                original.Publisher = book.Publisher;
                original.Published = book.Published;
                original.Isbn = book.Isbn;
                original.Image = book.Image;

                // updating indexes
                UpdateBookIndexes();

                return Copy(original);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Book> DeleteBookAsync(int id)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                if (!_books.ContainsKey(id))
                {
                    throw new NotFoundException(string.Format("Could not find book by id: {0}", id));
                }

                var book = _books[id];

                // remove from storage
                _books.Remove(id);

                // remove from sorting indexes
                _booksByTitle.Remove(book);
                _booksByPublishedDateDesc.Remove(book);
                UpdateBookIndexes();

                // updating author relations
                if (book.Authors != null)
                {
                    foreach (var author in book.Authors)
                    {
                        DeleteAuthorBookRelationship(book, author);
                    }
                }

                return book;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<Author>> GetAuthorsByNameAsync()
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Shared);
                var result = _authorsByName;

                // cloning authors
                return Copy(result);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Author> GetAuthorAsync(int id)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Shared);
                var author = _authors.ContainsKey(id) ? _authors[id] : null;
                return Copy(author);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                author = Copy(author);

                // set id
                author.Id = _authorIds++;

                // add to storage
                _authors.Add(author.Id, author);

                // update indexes
                _authorsByName.Add(author);
                UpdateAuthorIndexes();
            }
            finally
            {
                _lock.Release();
            }

            return author;
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                if (!_authors.ContainsKey(author.Id))
                {
                    throw new NotFoundException(string.Format("Could not find author by id: {0}", author.Id));
                }

                author = Copy(author);
                var original = _authors[author.Id];

                // updating author
                original.FirstName = author.FirstName;
                original.LastName = author.LastName;

                // updating indexes
                UpdateAuthorIndexes();

                return Copy(original);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<Author> DeleteAuthorAsync(int id)
        {
            try
            {
                await _lock.WaitAsync(OneManyMode.Exclusive);

                if (!_authors.ContainsKey(id))
                {
                    throw new NotFoundException(string.Format("Could not find author by id: {0}", id));
                }

                var author = _authors[id];

                // remove from storage
                _authors.Remove(id);

                // remove from sorting indexes
                _authorsByName.Remove(author);
                UpdateAuthorIndexes();

                // updating author relations
                var authorBooks = _authorBooks.ContainsKey(author) ? _authorBooks[author] : null;
                if (authorBooks != null)
                {
                    foreach (var book in authorBooks)
                    {
                        DeleteAuthorBookRelationship(book, author);
                    }
                }

                return author;
            }
            finally
            {
                _lock.Release();
            }
        }

        #region helpers

        private static Book Copy(Book book)
        {
            return book != null ? (Book) book.Clone() : null;
        }

        private static Author Copy(Author author)
        {
            return author != null ? (Author) author.Clone() : null;
        }

        private static List<Book> Copy(List<Book> books)
        {
            return books != null ? books.Select(b => (Book) b.Clone()).ToList() : null;
        }

        private static List<Author> Copy(List<Author> authors)
        {
            return authors != null ? authors.Select(b => (Author) b.Clone()).ToList() : null;
        }

        private void AddAuthorBookRelationship(Book book, Author author)
        {
            book = _books[book.Id];
            author = _authors[author.Id];

            if (_authorBooks.ContainsKey(author))
            {
                _authorBooks[author].Add(book);
            }
            else
            {
                _authorBooks.Add(author, new List<Book> {book});
            }
        }

        private void DeleteAuthorBookRelationship(Book book, Author author)
        {
            book = _books[book.Id];
            author = _authors[author.Id];

            var authorBooks = _authorBooks[author];
            authorBooks.Remove(book);
            if (!authorBooks.Any())
            {
                // no more books
                _authorBooks.Remove(author);
            }
        }

        private void CheckBookRelations(Book book)
        {
            if (book.Authors != null)
            {
                foreach (var author in book.Authors)
                {
                    if (author.Id <= 0 || !_authors.ContainsKey(author.Id))
                    {
                        throw new NotFoundException(string.Format("Could not find author {0} {1} with id: {2}",
                            author.FirstName, author.LastName, author.Id));
                    }
                }
            }
        }

        private void LoadBookRelations(Book book)
        {
            if (book.Authors != null)
            {
                var authors = book.Authors.Select(author => _authors[author.Id]).ToList();
                book.Authors = authors;
            }
        }

        private void UpdateBookIndexes()
        {
            _booksByTitle = _booksByTitle.OrderBy(b => b.Title).ToList();
            _booksByPublishedDateDesc = _booksByPublishedDateDesc.OrderByDescending(b => b.Published).ToList();
        }

        private void UpdateAuthorIndexes()
        {
            _authorsByName = _authorsByName.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
        }

        #endregion
    }
}