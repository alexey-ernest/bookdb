using System;
using System.Collections.Generic;
using BookDb.DAL;

namespace BookDb.Infrastructure.Initializers
{
    public class SeedInitializer : ISeedInitializer
    {
        private readonly IBookDatabase _db;

        public SeedInitializer(IBookDatabase db)
        {
            _db = db;
        }

        public void Initialize()
        {
            var author1 = new Author
            {
                FirstName = "Jeffrey",
                LastName = "Richter"
            };
            author1 = _db.AddAuthorAsync(author1).Result;

            var book1 = new Book
            {
                Title = "CLR via C# (4th Edition)",
                Pages = 896,
                Publisher = "Microsoft Press",
                Published = new DateTime(2012, 11, 25),
                Isbn = "978-0-735-66745-7",
                Image = "http://ecx.images-amazon.com/images/I/41zZ5aN3ypL.jpg",
                Authors = new List<Author>
                {
                    author1
                }
            };

            _db.AddBookAsync(book1).Wait();

            var author2 = new Author
            {
                FirstName = "Mark",
                LastName = "Lutz"
            };
            author2 = _db.AddAuthorAsync(author2).Result;

            var book2 = new Book
            {
                Title = "Learning Python, 5th Edition",
                Pages = 1600,
                Publisher = "O'Reilly Media",
                Published = new DateTime(2013, 7, 6),
                Isbn = "978-1-449-35573-9",
                Image = "http://ecx.images-amazon.com/images/I/515iBchIIzL._SX379_BO1,204,203,200_.jpg",
                Authors = new List<Author>
                {
                    author2
                }
            };

            _db.AddBookAsync(book2).Wait();

            var author3 = new Author
            {
                FirstName = "Mike",
                LastName = "Cantelon"
            };

            author3 = _db.AddAuthorAsync(author3).Result;

            var book3 = new Book
            {
                Title = "Node.js in Action",
                Pages = 416,
                Publisher = "Manning Publications",
                Published = new DateTime(2013, 11, 28),
                Isbn = "978-1-617-29057-2",
                Image = "http://ecx.images-amazon.com/images/I/51twwFigyiL._SX397_BO1,204,203,200_.jpg",
                Authors = new List<Author>
                {
                    author3
                }
            };

            _db.AddBookAsync(book3).Wait();
        }
    }
}