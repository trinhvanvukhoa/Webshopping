using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Please enter a product name.")]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Please enter a price.")]
        [Range(0.01, 99999999999999.99, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [StringLength(250, ErrorMessage = "Image path cannot exceed 250 characters.")]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}