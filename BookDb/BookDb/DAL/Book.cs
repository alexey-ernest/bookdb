using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookDb.DAL
{
    public class Book: ICloneable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        public List<Author> Authors { get; set; }

        [Required]
        [Range(1, 10000)]
        public int Pages { get; set; }

        [MaxLength(30)]
        public string Publisher { get; set; }

        // min 1800 year
        public DateTime Published { get; set; }

        // https://en.wikipedia.org/wiki/International_Standard_Book_Number
        public string Isbn { get; set; }

        public string Image { get; set; }


        public override int GetHashCode()
        {
            return Id;
        }

        public object Clone()
        {
            return new Book
            {
                Id = Id,
                Title = Title,
                Authors = Authors != null ? Authors.Select(a => (Author)a.Clone()).ToList() : null,
                Pages = Pages,
                Publisher = Publisher,
                Published = Published,
                Isbn = Isbn,
                Image = Image
            };
        }
    }
}