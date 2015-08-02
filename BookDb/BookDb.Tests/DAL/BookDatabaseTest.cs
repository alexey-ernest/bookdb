using System;
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
    }
}
