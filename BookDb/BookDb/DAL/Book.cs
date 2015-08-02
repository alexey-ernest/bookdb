using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BookDb.Infrastructure.Validation;

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

        [DateRange(MinDate = "1800-01-01")]
        public DateTime? Published { get; set; }

        [Display(Name = "ISBN 10 or 13")]
        [Isbn]
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