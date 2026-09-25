using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        // Khóa chính, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        // Khóa ngoại liên kết với Order
        [Required(ErrorMessage = "Vui lòng chọn đơn hàng")]
        [Display(Name = "Mã đơn hàng")]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        // Khóa ngoại liên kết với Product
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        // Số lượng sản phẩm mua
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        // Đơn giá tại thời điểm đặt hàng
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(0, int.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        [Display(Name = "Đơn giá")]
        public int UnitPrice { get; set; }
    }
}