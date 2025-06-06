﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Bookstore.Models
{
    public class Category

    {

        // PK
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "A customized error message")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property: A category can have many books (optional from the category side)

        [ValidateNever]
        public ICollection<Book> Books { get; set; } = [];

    }
}
