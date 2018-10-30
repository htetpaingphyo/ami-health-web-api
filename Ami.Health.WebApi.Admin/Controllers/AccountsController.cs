using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ami.Health.Core.Entities;
using Ami.Health.Core.Servies;
using Ami.Health.WebApi.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ami.Health.WebApi.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MainDbContext db;
        private readonly SecurityService service;

        public AccountsController(MainDbContext context)
        {
            db = context;
            service = new SecurityService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction("Login", "Home");

            var models = db.Users.Where(u => u.AccountStatus == AccountStatus.ACTIVE).OrderByDescending(u => u.CreatedDate).ToList();
            if (models == null)
                return NotFound();

            var users = new List<UserViewModel>();
            foreach (var model in models)
            {
                UserViewModel user = new UserViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    NRC = model.NRC,
                    DOB = model.DOB,
                    Email = model.Email,
                    UserCode = model.UserCode,
                    Phone = model.Phone,
                    Address = model.Address
                };
                users.Add(user);
            }

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(users);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction("Login", "Home");

            var user = db.Users.Where(u => u.Id == id).SingleOrDefault();

            if (user.IsFirstTimeLogin)
            {
                var item = db.Users.ToList().Where(u => u.Id == id).SingleOrDefault();
                var salt = db.Users.ToList().Where(u => u.Id == id).SingleOrDefault().PasswordSalt;
                item.Password = service.Decrypt(item.Password, salt);
            }
            else
            {
                user.Password = string.Empty;
            }

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction("Login", "Home");
            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userData = db.Users.SingleOrDefault(u => u.UserCode == model.UserCode);
            if (userData != null) return Forbid();

            var policyData = db.Users.Where(u => u.PolicyNo == model.PolicyNo).SingleOrDefault();
            if (policyData != null) return Forbid();

            model.Id = Guid.NewGuid();
            var salt = service.GetUniqueString;

            var user = new User
            {
                Id = model.Id,
                PolicyNo = model.PolicyNo,
                Name = model.Name,
                NRC = model.NRC,
                DOB = model.DOB,
                Email = model.Email,
                UserCode = model.UserCode,
                PasswordSalt = salt,
                Password = service.Encrypt(service.GetUniqueString, salt),
                Phone = model.Phone,
                Address = model.Address,
                IsFirstTimeLogin = true,
                AccountStatus = Core.Entities.AccountStatus.ACTIVE,
                CreatedDate = DateTime.Now
            };

            db.Users.Add(user);
            if (db.SaveChanges() > 0)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(Error));
        }

        [HttpGet]
        public IActionResult Lock(Guid id)
        {
            var user = db.Users.Where(u => u.Id == id).SingleOrDefault();
            if (user == null)
                return NotFound();

            user.AccountStatus = AccountStatus.LOCKED;
            user.UpdatedDate = DateTime.Now;

            db.Users.Update(user);
            if (db.SaveChanges() > 0)
                return RedirectToAction(nameof(Locked));
            else
                return BadRequest();
        }

        [HttpGet]
        public IActionResult Locked()
        {
            var session = HttpContext.Session.GetString("user");
            if (session is null) return RedirectToAction("Login", "Home");

            var lockedUsers = db.Users.Where<User>(user => user.AccountStatus == AccountStatus.LOCKED).ToList();
            if (lockedUsers == null)
                return NotFound();

            var lockedViews = new List<AccountStatusModel>();

            foreach (var user in lockedUsers)
            {
                var model = new AccountStatusModel
                {
                    Id = user.Id,
                    PolicyNo = user.PolicyNo,
                    Name = user.Name,
                    UserCode = user.UserCode,
                    NRC = user.NRC,
                    DOB = user.DOB,
                    Phone = user.Phone,
                    AccountStatus = user.AccountStatus
                };
                lockedViews.Add(model);
            }

            ViewData["User"] = GetUserNameBySession(Guid.Parse(session));
            return View(lockedViews);
        }

        [HttpGet]
        public IActionResult Unlock(Guid id)
        {
            var user = db.Users.Where(u => u.Id == id).SingleOrDefault();

            user.PasswordSalt = service.GetUniqueString;
            user.Password = service.Encrypt(service.GetUniqueString, user.PasswordSalt);
            user.AccountStatus = AccountStatus.ACTIVE;
            user.IsFirstTimeLogin = true;
            user.UpdatedDate = DateTime.Now;

            db.Users.Update(user);
            if (db.SaveChanges() > 0)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest();
        }

        [HttpGet]
        public IActionResult Deactivate(Guid id)
        {
            var user = db.Users.Where(u => u.Id == id).SingleOrDefault();
            user.AccountStatus = AccountStatus.INACTIVE;
            user.UpdatedDate = DateTime.Now;

            db.Users.Update(user);
            if (db.SaveChanges() > 0)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest();
        }

        public string GetUserNameBySession(Guid session)
        {
            var name = db.Admins.SingleOrDefault(u => u.Id == session).Name;
            return name is null ? string.Empty : name;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}