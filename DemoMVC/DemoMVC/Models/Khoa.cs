using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models;

public class Khoa
{
    [Key]
    [StringLength(2, MinimumLength = 2)]
    [Display(Name = "Mã khoa")]
    public string MaKhoa { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Tên khoa")]
    public string TenKhoa { get; set; } = string.Empty;

    public ICollection<Student> SinhViens { get; set; } = new List<Student>();
}
