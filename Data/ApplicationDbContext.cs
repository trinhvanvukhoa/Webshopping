using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Models;

namespace WebsiteShopping.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor nhận cấu hình kết nối từ Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Các bảng trong cơ sở dữ liệu
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }
    }
}