using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteShopping.Models
{
    [Table("User")]
    public class User
    {
        // Khóa chính, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // Tên đăng nhập
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        // Mật khẩu
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(255, ErrorMessage = "Mật khẩu không được vượt quá 255 ký tự")]
        [Column(TypeName = "varchar(255)")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        // Họ và tên
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        // Vai trò tài khoản
        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        [StringLength(20, ErrorMessage = "Vai trò không được vượt quá 20 ký tự")]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Vai trò")]
        public string Role { get; set; } = string.Empty;
    }
}