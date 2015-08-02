using BookDb.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookDb.Tests.DAL
{
    [TestClass]
    public class AuthorTest
    {
        [TestMethod]
        public void CloneTest()
        {
            var author = new Author
            {
                Id = 5,
                FirstName = "Jeff",
                LastName = "Richter"
            };

            var result = (Author)author.Clone();

            Assert.AreEqual(author.Id, result.Id);
            Assert.AreEqual(author.FirstName, result.FirstName);
            Assert.AreEqual(author.LastName, result.LastName);
        }
    }
}