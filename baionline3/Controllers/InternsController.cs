using baionline3.Data;
using baionline3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace baionline3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InternsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InternsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccessibleInterns()
        {
            // Lấy UserId từ JWT token
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            // Lấy RoleId của user
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return Unauthorized();
            var roleId = user.RoleId;

            // Lấy AccessProperties từ AllowAccess (theo RoleId, TableName="Intern")
            var access = _context.AllowAccesses.FirstOrDefault(a => a.RoleId == roleId && a.TableName == "Intern");
            if (access == null || string.IsNullOrWhiteSpace(access.AccessProperties))
                return Forbid("Bạn không có quyền xem bất kỳ cột nào!");

            // Parse AccessProperties thành danh sách cột
            var allowedColumns = access.AccessProperties.Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);

            // Lấy danh sách Interns
            var interns = _context.Interns.ToList();

            // Tạo danh sách kết quả động
            var result = interns.Select(i =>
            {
                var dict = new Dictionary<string, object?>();
                foreach (var col in allowedColumns)
                {
                    var prop = typeof(Intern).GetProperty(col);
                    if (prop != null)
                    {
                        dict[col] = prop.GetValue(i);
                    }
                }
                return dict;
            });

            return Ok(result);
        }
    }

}
