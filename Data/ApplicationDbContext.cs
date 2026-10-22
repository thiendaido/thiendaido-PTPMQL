using Microsoft.EntityFrameworkCore;
using DemoMVC.Models;

namespace DemoMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Khai báo các DbSet tương ứng với các bảng trong CSDL
        public DbSet<Student> Students { get; set; }
        public DbSet<Khoa> Khoas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chèn dữ liệu Khoa mẫu
            modelBuilder.Entity<Khoa>().HasData(
                new Khoa { MaKhoa = "05", TenKhoa = "Công nghệ thông tin" },
                new Khoa { MaKhoa = "01", TenKhoa = "Dầu khí" },
                new Khoa { MaKhoa = "02", TenKhoa = "Mỏ" },
                new Khoa { MaKhoa = "03", TenKhoa = "Trắc địa - Bản đồ và Quản lý đất đai" },
                new Khoa { MaKhoa = "04", TenKhoa = "Cơ - Điện" }
            );
        }
    }
}