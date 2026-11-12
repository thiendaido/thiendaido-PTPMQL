using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models;

public class Student
{
    [Key]
    [StringLength(10, MinimumLength = 10)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime? NgaySinh { get; set; }

    [Required]
    [StringLength(10)]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(256)]
    public string? Email { get; set; }

    [Required]
    [StringLength(2, MinimumLength = 2)]
    [Display(Name = "Mã khoa")]
    public string MaKhoa { get; set; } = string.Empty;

    [Required]
    [Range(2000, 9999)]
    [Display(Name = "Năm nhập học")]
    public int NamNhapHoc { get; set; } = DateTime.Today.Year;

    [Required]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "Mã hệ đào tạo phải gồm đúng 2 chữ số.")]
    [Display(Name = "Mã hệ đào tạo")]
    public string HeDaoTao { get; set; } = "21";

    [ForeignKey(nameof(MaKhoa))]
    public Khoa? Khoa { get; set; }
}
