using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

public class StudentController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var departments = await _context.Khoas
            .AsNoTracking()
            .OrderBy(khoa => khoa.TenKhoa)
            .ToListAsync();

        return View(departments);
    }

    [HttpGet]
    public async Task<IActionResult> List(string? search, int page = 1, int pageSize = 10)
    {
        pageSize = pageSize is 5 or 10 or 20 or 50 ? pageSize : 10;
        page = Math.Max(page, 1);

        var students = _context.Students
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLower();
            students = students.Where(student =>
                student.Id.ToLower().Contains(searchTerm)
                || student.HoTen.ToLower().Contains(searchTerm)
                || (student.NgaySinh != null && student.NgaySinh.Value.ToString().Contains(searchTerm))
                || (student.Email != null && student.Email.ToLower().Contains(searchTerm))
                || student.GioiTinh.ToLower().Contains(searchTerm)
                || student.MaKhoa.ToLower().Contains(searchTerm)
                || (student.Khoa != null && student.Khoa.TenKhoa.ToLower().Contains(searchTerm))
                || student.NamNhapHoc.ToString().Contains(searchTerm)
                || student.HeDaoTao.ToLower().Contains(searchTerm));
        }

        var totalItems = await students.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        page = Math.Min(page, totalPages);

        var items = await students
            .OrderBy(student => student.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(student => new
            {
                student.Id,
                student.HoTen,
                student.NgaySinh,
                student.GioiTinh,
                student.Email,
                student.MaKhoa,
                student.NamNhapHoc,
                student.HeDaoTao,
                TenKhoa = student.Khoa == null ? null : student.Khoa.TenKhoa
            })
            .ToListAsync();

        return Json(new { items, page, pageSize, totalItems, totalPages });
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Include(item => item.Khoa)
            .FirstOrDefaultAsync(item => item.Id == id);

        return student is null
            ? NotFound(new { message = "Không tìm thấy sinh viên." })
            : Json(ToStudentResponse(student));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] StudentFormModel? input)
    {
        if (input is null)
        {
            ModelState.AddModelError(string.Empty, "Yêu cầu không chứa dữ liệu hợp lệ.");
            return InvalidInput();
        }

        if (!TryValidateModel(input))
        {
            return InvalidInput();
        }

        if (!await _context.Khoas.AnyAsync(khoa => khoa.MaKhoa == input.MaKhoa))
        {
            ModelState.AddModelError(nameof(input.MaKhoa), "Khoa đã chọn không tồn tại.");
            return InvalidInput();
        }

        if (await _context.Students.AnyAsync(student => student.Id == input.Id))
        {
            ModelState.AddModelError(nameof(input.Id), "Mã sinh viên đã tồn tại.");
            return InvalidInput();
        }

        var student = new Student
        {
            Id = input.Id,
            HoTen = input.HoTen,
            NgaySinh = input.NgaySinh,
            GioiTinh = input.GioiTinh,
            Email = input.Email,
            MaKhoa = input.MaKhoa,
            NamNhapHoc = input.NamNhapHoc,
            HeDaoTao = input.HeDaoTao
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Thêm sinh viên thành công." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string id, [FromBody] StudentFormModel? input)
    {
        if (input is null)
        {
            ModelState.AddModelError(string.Empty, "Yêu cầu không chứa dữ liệu hợp lệ.");
            return InvalidInput();
        }

        if (!TryValidateModel(input))
        {
            return InvalidInput();
        }

        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound(new { message = "Không tìm thấy sinh viên." });
        }

        if (!await _context.Khoas.AnyAsync(khoa => khoa.MaKhoa == input.MaKhoa))
        {
            ModelState.AddModelError(nameof(input.MaKhoa), "Khoa đã chọn không tồn tại.");
            return InvalidInput();
        }

        student.HoTen = input.HoTen;
        student.NgaySinh = input.NgaySinh;
        student.GioiTinh = input.GioiTinh;
        student.Email = input.Email;
        student.MaKhoa = input.MaKhoa;
        student.NamNhapHoc = input.NamNhapHoc;
        student.HeDaoTao = input.HeDaoTao;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật sinh viên thành công." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound(new { message = "Không tìm thấy sinh viên." });
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xóa sinh viên thành công." });
    }

    [HttpGet]
    public IActionResult DemoState()
    {
        ViewData["HeaderTitle"] = "Thực hành Quản lý Trạng thái";
        ViewData["ItemCount"] = 10;
        ViewBag.Message = "Dữ liệu này được truyền từ ViewBag!";
        ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitState(string studentName)
    {
        if (string.IsNullOrWhiteSpace(studentName))
        {
            TempData["ErrorMessage"] = "Vui lòng nhập tên sinh viên!";
            return RedirectToAction(nameof(DemoState));
        }

        TempData["SuccessMessage"] = $"Thêm sinh viên '{studentName}' thành công!";
        return RedirectToAction(nameof(DemoState));
    }

    private IActionResult InvalidInput()
    {
        var errors = ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                        ? "Giá trị không hợp lệ."
                        : error.ErrorMessage)
                    .ToArray());

        return BadRequest(new
        {
            message = "Vui lòng kiểm tra lại thông tin đã nhập.",
            errors
        });
    }

    private static object ToStudentResponse(Student student) => new
    {
        student.Id,
        student.HoTen,
        student.NgaySinh,
        student.GioiTinh,
        student.Email,
        student.MaKhoa,
        student.NamNhapHoc,
        student.HeDaoTao,
        TenKhoa = student.Khoa?.TenKhoa
    };
}
