using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

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

        [Display(Name = "Trường học")]
        public string Truong { get; set; } = string.Empty;
    }
}