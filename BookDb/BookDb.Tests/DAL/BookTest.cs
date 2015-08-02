using System;
using System.Collections.Generic;
using System.Linq;
using BookDb.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookDb.Tests.DAL
{
    [TestClass]
    public class BookTest
    {
        [TestMethod]
        public void CloneTest()
        {
            var book = new Book
            {
                Id = 7,
                Pages = 51,
                Published = DateTime.Now.AddYears(-5),
                Isbn = Guid.NewGuid().ToString(),
                Publisher = "Alpina Books",
                Title = "CLR via C#",
                Image = "https://www.google.ru/images/srpr/logo11w.png",
                Authors = new List<Author>
                {
                    new Author
                    {
                        Id = 5,
                        FirstName = "Jeff",
                        LastName = "Richter"
                    },
                    new Author
                    {
                        Id = 7,
                        FirstName = "Bill",
                        LastName = "Gates"
                    }
                }
            };

            var result = (Book) book.Clone();

            Assert.AreEqual(book.Id, result.Id);
            Assert.AreEqual(book.Pages, result.Pages);
            Assert.AreEqual(book.Published, result.Published);
            Assert.AreEqual(book.Isbn, result.Isbn);
            Assert.AreEqual(book.Publisher, result.Publisher);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Image, result.Image);
            Assert.AreEqual(book.Authors.Count, result.Authors.Count);

            foreach (var author in result.Authors)
            {
                CollectionAssert.DoesNotContain(book.Authors, author);
                Assert.IsTrue(book.Authors.Any(a => a.Id == author.Id));
            }
        }
    }
}