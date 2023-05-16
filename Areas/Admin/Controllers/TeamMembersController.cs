using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;

        public TeamMembersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TeamMembers> members = await _context.TeamMembers.ToListAsync();
            return View(members);
            //return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMembers member)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            await _context.TeamMembers.AddAsync(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            TeamMembers member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TeamMembers member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            TeamMembers result = await _context.TeamMembers.FirstOrDefaultAsync(t => t.Id == member.Id);
            if (result is null)
            {
                TempData["Exists"] = "Bu Member bazada yoxdur";
                return RedirectToAction(nameof(Index));
            }
            result.FullName = member.FullName;
            result.Profession = member.Profession;
            result.ImagePath = member.ImagePath;
            _context.TeamMembers.Update(result);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
            TeamMembers? member = _context.TeamMembers.Find(Id);
            if (member == null)
            {
                return NotFound();
            }
            _context.TeamMembers.Remove(member);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
