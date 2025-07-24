using baionline3.Data;
using baionline3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace baionline3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AllowAccessesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AllowAccessesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllowAccess>>> GetAllowAccesses()
        {
            return await _context.AllowAccesses.Include(a => a.Role).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AllowAccess>> GetAllowAccess(int id)
        {
            var allowAccess = await _context.AllowAccesses.Include(a => a.Role).FirstOrDefaultAsync(a => a.Id == id);
            if (allowAccess == null) return NotFound();
            return allowAccess;
        }

        [HttpPost]
        public async Task<ActionResult<AllowAccess>> PostAllowAccess(AllowAccess allowAccess)
        {
            if (!_context.Roles.Any(r => r.RoleId == allowAccess.RoleId))
                return BadRequest("RoleId không tồn tại.");

            _context.AllowAccesses.Add(allowAccess);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllowAccess), new { id = allowAccess.Id }, allowAccess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllowAccess(int id, AllowAccess allowAccess)
        {
            if (id != allowAccess.Id) return BadRequest();
            _context.Entry(allowAccess).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllowAccess(int id)
        {
            var allowAccess = await _context.AllowAccesses.FindAsync(id);
            if (allowAccess == null) return NotFound();

            _context.AllowAccesses.Remove(allowAccess);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
