﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Bookstore.Models
{
    public class Book

    {
        // process is to create models first (PK, FK, Navigation Property), then add the db connection in app settings, and finally create the database using migrations and update-database command
        // PK

        public int BookId { get; set; }

        [Required]
        [MaxLength(100)]

        public string Author { get; set; } = string.Empty;


        [Required]
        [MaxLength(200)]

        public string Title { get; set; } = string.Empty;

        public string? Image { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "The price must be greater than zero and between 0.01 and 10000.")]
        [DisplayFormat(DataFormatString = "{0:c}")]

        public decimal Price { get; set; }

        [Display(Name = "Mature Content")]

        public bool MatureContent { get; set; }

        // FK: Every book must have a category (mandatory)

        [Required]

        [Display(Name = "Category")]

        public int CategoryId { get; set; }



        // Navigation property: each book belongs to one category (mandatory from the book side)
        [ValidateNever]
        public Category Category { get; set; } = null!;



        // Navigation property: Book may appear in many CartItems (optional from the book side)
        [ValidateNever]
        public ICollection<CartItem> CartItems { get; set; } = [];



        // Navigation property: Book may appear in many OrderDetails (optional from the book side)
        [ValidateNever]
        public ICollection<OrderDetail> OrderDetails { get; set; } = [];

    }
}
