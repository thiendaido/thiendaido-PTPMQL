using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Tắt tự động tăng vì sẽ tự sinh mã sinh viên
        [Display(Name = "Mã Sinh Viên")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")]
        [Display(Name = "Họ và Tên")]
        public string HoTen { get; set; } = string.Empty;

        [Range(18, 100, ErrorMessage = "Tuổi phải từ 18 đến 100")]
        [Display(Name = "Tuổi")]
        public int Tuoi { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = string.Empty;

        // Khóa ngoại liên kết tới bảng Khoa
        [Required(ErrorMessage = "Vui lòng chọn Khoa")]
        [Display(Name = "Khoa")]
        public string MaKhoa { get; set; } = string.Empty;

        [ForeignKey("MaKhoa")]
        public Khoa? Khoa { get; set; }
    }
}