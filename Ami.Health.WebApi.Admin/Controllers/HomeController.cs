using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ami.Health.WebApi.Admin.Models;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Ami.Health.WebApi.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly MainDbContext _context;

        public HomeController(MainDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction(nameof(Login));

            List<AdminViewModel> models = new List<AdminViewModel>();
            var admins = await _context.Admins.ToListAsync();

            foreach (var admin in admins)
            {
                var model = new AdminViewModel
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    Email = admin.Email,
                    Password = admin.Password,
                    Designation = admin.Designation,
                    Department = admin.Department
                };
                models.Add(model);
            }

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(models);
        }

        public IActionResult Login()
        {
            var session = HttpContext.Session.GetString("user");
            if (session != null) return RedirectToAction("Index", "Accounts");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")]LoginViewModel login)
        {
            var session = HttpContext.Session.GetString("user");

            if (ModelState.IsValid)
            {
                var user = await _context.Admins.Where(admin => admin.Email == login.Email).SingleOrDefaultAsync();
                if (user is null) return NotFound();

                var md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = Encoding.UTF8.GetBytes(login.Password);
                var hashPassword = Encoding.UTF8.GetString(md5.ComputeHash(inputBytes));

                if (user.Password == hashPassword)
                {
                    if (session is null)
                        HttpContext.Session.SetString("user", user.Id.ToString());

                    return RedirectToAction("Index", "Accounts");
                }
            }
            return View(login);
        }

        public IActionResult Logout()
        {
            var session = HttpContext.Session.Get("user");
            if (session is null) return RedirectToAction(nameof(Login));

            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction(nameof(Login));

            if (id == null) return NotFound();
            var admin = await _context.Admins.SingleOrDefaultAsync(u => u.Id == id);
            if (admin is null) return NotFound();

            var adminViewModel = new AdminViewModel
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                Password = admin.Password,
                Designation = admin.Designation,
                Department = admin.Department
            };

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(adminViewModel);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction(nameof(Login));
            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Password,Designation,Department,Id")] AdminViewModel adminViewModel)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var passBytes = Encoding.UTF8.GetBytes(adminViewModel.Password);

            if (ModelState.IsValid)
            {
                adminViewModel.Id = Guid.NewGuid();
                adminViewModel.Password = Encoding.UTF8.GetString(md5.ComputeHash(passBytes));

                var admin = new Models.Admin
                {
                    Id = adminViewModel.Id,
                    Name = adminViewModel.Name,
                    Email = adminViewModel.Email,
                    Password = adminViewModel.Password,
                    Designation = adminViewModel.Designation,
                    Department = adminViewModel.Department,
                    CreatedDate = DateTime.Now
                };

                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminViewModel);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction(nameof(Login));

            if (id == null) return NotFound();
            var admin = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            if (admin is null) return NotFound();

            var adminViewModel = new AdminViewModel
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                Password = admin.Password,
                Designation = admin.Designation,
                Department = admin.Department
            };

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(adminViewModel);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Email,Password,Designation,Department,Id")] AdminViewModel adminViewModel)
        {
            if (id != adminViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = await _context.Admins.Where(u => u.Id == adminViewModel.Id).SingleOrDefaultAsync();
                    if (admin is null) return NotFound();

                    admin.Id = adminViewModel.Id;
                    admin.Name = adminViewModel.Name;
                    admin.Email = adminViewModel.Email;
                    admin.Password = adminViewModel.Password;
                    admin.Designation = adminViewModel.Designation;
                    admin.Department = adminViewModel.Department;
                    admin.UpdatedDate = DateTime.Now;

                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(adminViewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminViewModel);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction(nameof(Login));

            if (id == null) return NotFound();
            var admin = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            if (admin is null) return NotFound();

            var adminViewModel = new AdminViewModel
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                Password = admin.Password,
                Designation = admin.Designation,
                Department = admin.Department
            };
            return View(adminViewModel);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var admin = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(Guid id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }

        public string GetUserNameBySession(Guid session)
        {
            var name = _context.Admins.SingleOrDefault(u => u.Id == session).Name;
            return name is null ? string.Empty : name;
        }
    }
}