using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models;

public class StudentFormModel : IValidatableObject
{
    [Required(ErrorMessage = "Vui lòng nhập mã sinh viên.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Mã sinh viên phải gồm đúng 10 ký tự.")]
    [Display(Name = "Mã sinh viên")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn ngày sinh.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime? NgaySinh { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(256, ErrorMessage = "Email không được vượt quá 256 ký tự.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn khoa.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Mã khoa phải gồm 2 ký tự.")]
    [Display(Name = "Khoa")]
    public string MaKhoa { get; set; } = string.Empty;

    [Range(2000, 9999, ErrorMessage = "Năm nhập học phải từ 2000 đến 9999.")]
    [Display(Name = "Năm nhập học")]
    public int NamNhapHoc { get; set; } = DateTime.Today.Year;

    [Required(ErrorMessage = "Vui lòng nhập mã hệ đào tạo.")]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "Mã hệ đào tạo phải gồm đúng 2 chữ số.")]
    [Display(Name = "Mã hệ đào tạo")]
    public string HeDaoTao { get; set; } = "21";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Student.GioiTinhOptions.Contains(GioiTinh))
        {
            yield return new ValidationResult(
                "Vui lòng chọn giới tính hợp lệ.",
                [nameof(GioiTinh)]);
        }
    }
}
