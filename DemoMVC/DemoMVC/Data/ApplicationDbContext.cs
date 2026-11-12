using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Khoa> Khoas => Set<Khoa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasOne(student => student.Khoa)
            .WithMany(khoa => khoa.SinhViens)
            .HasForeignKey(student => student.MaKhoa)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Khoa>().HasData(
            new Khoa { MaKhoa = "05", TenKhoa = "Công nghệ thông tin" },
            new Khoa { MaKhoa = "01", TenKhoa = "Dầu khí" },
            new Khoa { MaKhoa = "02", TenKhoa = "Mỏ" },
            new Khoa { MaKhoa = "03", TenKhoa = "Trắc địa - Bản đồ và Quản lý đất đai" },
            new Khoa { MaKhoa = "04", TenKhoa = "Cơ - Điện" });
    }
}
