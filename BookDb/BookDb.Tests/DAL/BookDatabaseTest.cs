using System;
using System.Collections.Generic;
using System.Linq;
using BookDb.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookDb.Tests.DAL
{
    [TestClass]
    public class BookDatabaseTest
    {
        private IBookDatabase _db;

        [TestInitialize]
        public void Initialize()
        {
            _db = new BookDatabase();
        }

        [TestMethod]
        public void AddAuthorTest()
        {
            var author = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var result = _db.AddAuthorAsync(author).Result;

            Assert.AreNotSame(author, result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(author.FirstName, result.FirstName);
            Assert.AreEqual(author.LastName, result.LastName);
        }

        [TestMethod]
        public void GetAuthorTest()
        {
            var author = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            author = _db.AddAuthorAsync(author).Result;

            var result = _db.GetAuthorAsync(author.Id).Result;

            Assert.AreNotSame(author, result);
            Assert.AreEqual(author.Id, result.Id);
            Assert.AreEqual(author.FirstName, result.FirstName);
            Assert.AreEqual(author.LastName, result.LastName);
        }

        [TestMethod]
        public void AddTwoAuthorsTest()
        {
            var author1 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var author2 = new Author
            {
                FirstName = "Steve",
                LastName = "Jobs"
            };

            var result1 = _db.AddAuthorAsync(author1).Result;
            var result2 = _db.AddAuthorAsync(author2).Result;

            Assert.IsTrue(result1.Id > 0);
            Assert.IsTrue(result2.Id > 0);
            Assert.AreNotEqual(result1.Id, result2.Id);
        }

        [TestMethod]
        public void AuthorsByNameTest()
        {
            var author1 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var author2 = new Author
            {
                FirstName = "Elon",
                LastName = "Musk"
            };

            var author3 = new Author
            {
                FirstName = "Steve",
                LastName = "Jobs"
            };

            _db.AddAuthorAsync(author1).Wait();
            _db.AddAuthorAsync(author2).Wait();
            _db.AddAuthorAsync(author3).Wait();

            var authors = _db.GetAuthorsByNameAsync().Result;

            Assert.AreEqual(3, authors.Count);
            Assert.AreEqual(author1.LastName, authors[0].LastName);
            Assert.AreEqual(author3.LastName, authors[1].LastName);
            Assert.AreEqual(author2.LastName, authors[2].LastName);
        }

        [TestMethod]
        public void UpdateAuthorWithUnexistingIdTest()
        {
            var author = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var result = _db.AddAuthorAsync(author).Result;
            result.Id++;

            try
            {
                _db.UpdateAuthorAsync(result).Wait();
            }
            catch (Exception)
            {
                return;
            }
            
            Assert.Fail();
        }

        [TestMethod]
        public void UpdateAuthorTest()
        {
            var author = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            author = _db.AddAuthorAsync(author).Result;
            author.FirstName = "Elon";
            author.LastName = "Musk";

            var result = _db.UpdateAuthorAsync(author).Result;

            Assert.AreEqual(author.Id, result.Id);
            Assert.AreNotSame(author, result);
            Assert.AreEqual(author.FirstName, result.FirstName);
            Assert.AreEqual(author.LastName, result.LastName);
        }

        [TestMethod]
        public void AuthorsByNameUpdateTest()
        {
            var author1 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var author2 = new Author
            {
                FirstName = "Elon",
                LastName = "Musk"
            };

            var author3 = new Author
            {
                FirstName = "Steve",
                LastName = "Jobs"
            };

            author1 = _db.AddAuthorAsync(author1).Result;
            _db.AddAuthorAsync(author2).Wait();
            _db.AddAuthorAsync(author3).Wait();

            author1.FirstName = "Satya";
            author1.LastName = "Nadella";
            _db.UpdateAuthorAsync(author1).Wait();

            var authors = _db.GetAuthorsByNameAsync().Result;

            Assert.AreEqual(3, authors.Count);
            Assert.AreEqual(author3.LastName, authors[0].LastName);
            Assert.AreEqual(author2.LastName, authors[1].LastName);
            Assert.AreEqual(author1.LastName, authors[2].LastName);
        }

        [TestMethod]
        public void DeleteAuthorTest()
        {
            var author1 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var author2 = new Author
            {
                FirstName = "Elon",
                LastName = "Musk"
            };

            author1 = _db.AddAuthorAsync(author1).Result;
            author2 = _db.AddAuthorAsync(author2).Result;

            _db.DeleteAuthorAsync(author1.Id).Wait();

            var authors = _db.GetAuthorsByNameAsync().Result;

            Assert.AreEqual(1, authors.Count);
            Assert.AreEqual(author2.Id, authors[0].Id);
        }

        [TestMethod]
        public void AuthorsByNameDeleteTest()
        {
            var author1 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };

            var author2 = new Author
            {
                FirstName = "Elon",
                LastName = "Musk"
            };

            var author3 = new Author
            {
                FirstName = "Steve",
                LastName = "Jobs"
            };

            author1 = _db.AddAuthorAsync(author1).Result;
            author2 = _db.AddAuthorAsync(author2).Result;
            author3 = _db.AddAuthorAsync(author3).Result;

            _db.DeleteAuthorAsync(author3.Id).Wait();

            var authors = _db.GetAuthorsByNameAsync().Result;

            Assert.AreEqual(2, authors.Count);
            Assert.AreEqual(author1.LastName, authors[0].LastName);
            Assert.AreEqual(author2.LastName, authors[1].LastName);
        }

        [TestMethod]
        public void AddBookWithoutAuthorsTest()
        {
            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png"
            };

            var result = _db.AddBookAsync(book).Result;

            Assert.IsTrue(result.Id > 0);
            Assert.AreNotSame(book, result);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Pages, result.Pages);
            Assert.AreEqual(book.Publisher, result.Publisher);
            Assert.AreEqual(book.Published, result.Published);
            Assert.AreEqual(book.Isbn, result.Isbn);
            Assert.AreEqual(book.Image, result.Image);
        }

        [TestMethod]
        public void AddBookWithUnexistingAuthorTest()
        {
            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png",
                Authors = new List<Author>
                {
                    new Author()
                }
            };

            try
            {
                _db.AddBookAsync(book).Wait();
            }
            catch (Exception)
            {
                return;
            }
            
            Assert.Fail();
        }

        [TestMethod]
        public void BooksByTitleTest()
        {
            var book1 = new Book
            {
                Title = "Learning Python",
                Pages = 1400,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-10),
                Isbn = Guid.NewGuid().ToString()
            };

            var book2 = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png"
            };

            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;

            var result = _db.GetBooksByTitleAsync().Result;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(book2.Title, result[0].Title);
            Assert.AreEqual(book1.Title, result[1].Title);
        }

        [TestMethod]
        public void BooksByPublishedDateTest()
        {
            var book1 = new Book
            {
                Title = "Learning Python",
                Pages = 1400,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-10),
                Isbn = Guid.NewGuid().ToString()
            };

            var book2 = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png"
            };

            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;

            var result = _db.GetBooksByPublishedDateAsync().Result;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(book2.Title, result[0].Title);
            Assert.AreEqual(book1.Title, result[1].Title);
        }

        [TestMethod]
        public void AddBookWithAuthorTest()
        {
            var author = new Author
            {
                FirstName = "Jeff",
                LastName = "Richter"
            };
            author = _db.AddAuthorAsync(author).Result;

            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png",
                Authors = new List<Author>
                {
                    author
                }
            };

            var result = _db.AddBookAsync(book).Result;

            Assert.AreEqual(1, result.Authors.Count);
            Assert.AreNotSame(author, result.Authors[0]);
            Assert.AreEqual(author.LastName, result.Authors[0].LastName);
        }

        [TestMethod]
        public void GetBookTest()
        {
            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png"
            };

            book = _db.AddBookAsync(book).Result;

            var result = _db.GetBookAsync(book.Id).Result;

            Assert.AreNotSame(book, result);
            Assert.AreEqual(book.Id, result.Id);
            Assert.AreEqual(book.Title, result.Title);
        }

        [TestMethod]
        public void UpdateBookWithUnexistingIdTest()
        {
            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png"
            };

            book = _db.AddBookAsync(book).Result;
            book.Id++;

            try
            {
                _db.UpdateBookAsync(book).Wait();
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void UpdateBookWithUnexistingAuthorTest()
        {
            var author = new Author
            {
                FirstName = "Jeff",
                LastName = "Richter"
            };
            author = _db.AddAuthorAsync(author).Result;

            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png",
                Authors = new List<Author>
                {
                    author
                }
            };

            book = _db.AddBookAsync(book).Result;

            book.Authors[0].Id++;

            try
            {
                _db.UpdateBookAsync(book).Wait();
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void UpdateBookTest()
        {
            var author1 = new Author
            {
                FirstName = "Jeff",
                LastName = "Richter"
            };
            author1 = _db.AddAuthorAsync(author1).Result;

            var author2 = new Author
            {
                FirstName = "Bill",
                LastName = "Gates"
            };
            author2 = _db.AddAuthorAsync(author2).Result;

            var book = new Book
            {
                Title = "CLR via C#",
                Pages = 900,
                Publisher = "IT Books",
                Published = DateTime.Now.Date.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Image = "https://www.google.ru/images/srpr/logo11w.png",
                Authors = new List<Author>
                {
                    author1
                }
            };

            book = _db.AddBookAsync(book).Result;

            book.Title += "2";
            book.Pages += 20;
            book.Publisher += "2";
            book.Published = book.Published.AddYears(2);
            book.Isbn += "2";
            book.Image += "2";
            book.Authors = new List<Author> { author2 };

            var result = _db.UpdateBookAsync(book).Result;

            Assert.AreNotSame(book, result);
            Assert.AreEqual(book.Id, result.Id);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Pages, result.Pages);
            Assert.AreEqual(book.Publisher, result.Publisher);
            Assert.AreEqual(book.Isbn, result.Isbn);
            Assert.AreEqual(book.Image, result.Image);
            Assert.AreEqual(1, result.Authors.Count);
            Assert.IsTrue(result.Authors.Any(a => a.Id == author2.Id ));
        }

        [TestMethod]
        public void BooksByTitleUpdateTest()
        {
            var book1 = new Book
            {
                Title = "Node.js in Action"
            };
            var book2 = new Book
            {
                Title = "CLR via C#"
            };
            var book3 = new Book
            {
                Title = "Learning Python"
            };
            
            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;
            book3 = _db.AddBookAsync(book3).Result;

            book1.Title = "A " + book1.Title;
            book1 = _db.UpdateBookAsync(book1).Result;

            var result = _db.GetBooksByTitleAsync().Result;

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(book1.Title, result[0].Title);
            Assert.AreEqual(book2.Title, result[1].Title);
            Assert.AreEqual(book3.Title, result[2].Title);
        }

        [TestMethod]
        public void BooksByPublishedDateUpdateTest()
        {
            var book1 = new Book
            {
                Title = "Node.js in Action",
                Published = DateTime.Now.Date.AddYears(-4)
            };
            var book2 = new Book
            {
                Title = "CLR via C#",
                Published = DateTime.Now.Date.AddYears(-3)
            };
            var book3 = new Book
            {
                Title = "Learning Python",
                Published = DateTime.Now.Date.AddYears(-10)
            };

            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;
            book3 = _db.AddBookAsync(book3).Result;

            book2.Published = book2.Published.AddYears(-2);
            book2 = _db.UpdateBookAsync(book2).Result;

            var result = _db.GetBooksByPublishedDateAsync().Result;

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(book1.Title, result[0].Title);
            Assert.AreEqual(book2.Title, result[1].Title);
            Assert.AreEqual(book3.Title, result[2].Title);
        }

        [TestMethod]
        public void DeleteBookTest()
        {
            var book1 = new Book
            {
                Title = "Node.js in Action",
                Published = DateTime.Now.Date.AddYears(-4)
            };
            var book2 = new Book
            {
                Title = "CLR via C#",
                Published = DateTime.Now.Date.AddYears(-3)
            };
            var book3 = new Book
            {
                Title = "Learning Python",
                Published = DateTime.Now.Date.AddYears(-10)
            };

            _db.AddBookAsync(book1).Wait();
            book2 = _db.AddBookAsync(book2).Result;
            _db.AddBookAsync(book3).Wait();

            _db.DeleteBookAsync(book2.Id).Wait();

            var result = _db.GetBooksByTitleAsync().Result;

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(b => b.Id != book2.Id));
        }

        [TestMethod]
        public void BooksByTitleDeleteTest()
        {
            var book1 = new Book
            {
                Title = "Node.js in Action"
            };
            var book2 = new Book
            {
                Title = "CLR via C#"
            };
            var book3 = new Book
            {
                Title = "Learning Python"
            };

            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;
            book3 = _db.AddBookAsync(book3).Result;

            _db.DeleteBookAsync(book1.Id);

            var result = _db.GetBooksByTitleAsync().Result;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(book2.Title, result[0].Title);
            Assert.AreEqual(book3.Title, result[1].Title);
        }

        [TestMethod]
        public void BooksByPublishedDateDeleteTest()
        {
            var book1 = new Book
            {
                Title = "Node.js in Action",
                Published = DateTime.Now.Date.AddYears(-4)
            };
            var book2 = new Book
            {
                Title = "CLR via C#",
                Published = DateTime.Now.Date.AddYears(-3)
            };
            var book3 = new Book
            {
                Title = "Learning Python",
                Published = DateTime.Now.Date.AddYears(-10)
            };

            book1 = _db.AddBookAsync(book1).Result;
            book2 = _db.AddBookAsync(book2).Result;
            book3 = _db.AddBookAsync(book3).Result;

            _db.DeleteBookAsync(book2.Id).Wait();

            var result = _db.GetBooksByPublishedDateAsync().Result;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(book1.Title, result[0].Title);
            Assert.AreEqual(book3.Title, result[1].Title);
        }
    }
}
