using System;
using BookDb.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookDb.Tests.DAL
{
    [TestClass]
    public class BookRepositoryTest
    {
        private IBookRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new BookRepository(new BookDatabase());
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

            try
            {
                _repository.AddAsync(book).Wait();
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail();
        }
    }
}