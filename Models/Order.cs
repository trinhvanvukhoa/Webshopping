using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("Order")]
    public class Order
    {
        // Khóa chính, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // Họ tên người đặt hàng
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ tên khách hàng")]
        public string CustomerName { get; set; } = string.Empty;

        // Số điện thoại nhận hàng
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số")]
        [Column(TypeName = "varchar(15)")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Địa chỉ giao hàng
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string Address { get; set; } = string.Empty;

        // Ghi chú đơn hàng
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        // Ngày giờ tạo đơn hàng
        [Required]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        // Trạng thái đơn hàng
        [Required(ErrorMessage = "Vui lòng chọn trạng thái đơn hàng")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        [Display(Name = "Trạng thái đơn hàng")]
        public string Status { get; set; } = "Chờ xử lý";
    }
}