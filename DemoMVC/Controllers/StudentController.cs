using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers
{
    public class StudentController : Controller
    {
        // 1. Action hiển thị và demo ViewData, ViewBag, TempData
        [HttpGet]
        public IActionResult DemoState()
        {
            // Gán dữ liệu vào ViewData
            ViewData["HeaderTitle"] = "Thực hành Quản lý Trạng thái";
            ViewData["ItemCount"] = 10;

            // Gán dữ liệu vào ViewBag
            ViewBag.Message = "Dữ liệu này được truyền từ ViewBag!";
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }

        // 2. Action nhận Submit Form và dùng TempData để chuyển hướng (Redirect)
        [HttpPost]
        public IActionResult SubmitState(string studentName)
        {
            if (string.IsNullOrEmpty(studentName))
            {
                // Thông báo lỗi nếu chưa nhập tên
                TempData["ErrorMessage"] = "Vui lòng nhập tên sinh viên!";
                return RedirectToAction("DemoState");
            }

            // Lưu thông báo thành công vào TempData để chuyển sang trang DemoState
            TempData["SuccessMessage"] = $"Thêm sinh viên '{studentName}' thành công!";
            
            // Redirect sang Get Action (Request thứ 2)
            return RedirectToAction("DemoState");
        }
    }
}