using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Khoa
    {
        [Key]
        [Display(Name = "Mã Khoa")]
        public string MaKhoa { get; set; } = string.Empty; // VD: CNTT, CK, DDK

        [Required(ErrorMessage = "Tên khoa không được để trống")]
        [Display(Name = "Tên Khoa")]
        public string TenKhoa { get; set; } = string.Empty;

        // Navigation property: Một khoa có nhiều sinh viên
        public ICollection<Student>? Students { get; set; }
    }
}