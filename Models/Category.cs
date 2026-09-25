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
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; } = string.Empty;

        // Mô tả danh mục
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        // Trạng thái danh mục
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = string.Empty;
    }
}