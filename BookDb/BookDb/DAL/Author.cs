using System;
using System.ComponentModel.DataAnnotations;

namespace BookDb.DAL
{
    public class Author: ICloneable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }


        public override int GetHashCode()
        {
            return Id;
        }

        public object Clone()
        {
            return new Author
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
}