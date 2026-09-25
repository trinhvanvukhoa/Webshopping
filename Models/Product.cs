using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("Product")]
    public class Product
    {
        // Khóa chính, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        // Khóa ngoại tham chiếu đến Category
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // Tên sản phẩm
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        // Giá sản phẩm
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(0.01, 99999999999999.99, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá sản phẩm")]
        public decimal Price { get; set; }

        // Tên hoặc đường dẫn hình ảnh
        [StringLength(250, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 250 ký tự")]
        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        // Mô tả chi tiết sản phẩm
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Mô tả sản phẩm")]
        public string? Description { get; set; }
    }
}