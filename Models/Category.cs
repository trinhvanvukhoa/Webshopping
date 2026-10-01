using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("Category")]
    public class Category
    {
        // Khóa chính, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        // Tên danh mục
        [Required(ErrorMessage = "Please enter a category name.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = string.Empty;

        // Mô tả danh mục
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Trạng thái danh mục
        [Required(ErrorMessage = "Please select a status.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
    }
}