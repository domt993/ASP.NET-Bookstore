using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Bookstore.Models
{
    public class Categories
    {
        [Key]
        //[Range(1, 500, ErrorMessage = "Category ID must be between 1 and 100")]
        public int CategoryId { get; set; }
        [Display(Name = "Category Name", Prompt = "Technology")]
        [Required(ErrorMessage = "Please enter a category name")]
        public string Name { get; set; }
    }
}
