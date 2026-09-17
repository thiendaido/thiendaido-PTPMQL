using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public int Tuoi { get; set; }
        public string DiaChi { get; set; } = string.Empty;
        public string Truong { get; set; } = string.Empty;
    }
}