using System;
using System.Linq;
using System.Text.RegularExpressions;
using Ami.Health.Core.Entities;
using Ami.Health.Core.Servies;
using Ami.Health.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ami.Health.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("Accounts/[action]")]
    public class AccountsController : Controller
    {
        private readonly MainDbContext db;
        private readonly SecurityService service;
        private const string SESSION_KEY = "S_KEY";
        private int session_count = 0;
        private readonly IConfiguration _configuration;
        ILogger<AccountsController> _logger;        

        public AccountsController(MainDbContext context, IConfiguration configuration, ILogger<AccountsController> logger)
        {
            _logger = logger;
            db = context;
            _configuration = configuration;
            service = new SecurityService();
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            // Get counter info from session configuration.
            int s_count = Convert.ToInt16(_configuration["Session"]);

            if (!ModelState.IsValid)
                return BadRequest();

            // Check if session is existed!
            var session = HttpContext.Session.GetString(SESSION_KEY);
            if (string.IsNullOrEmpty(session))
            {
                HttpContext.Session.SetString(SESSION_KEY, model.UserCode);
            }

            var user = db.Users.Where(u =>
                            u.UserCode == model.UserCode &&
                            u.AccountStatus != AccountStatus.INACTIVE
                        ).SingleOrDefault();
            if (user is null)
                return NotFound();

            user.Password = service.Decrypt(user.Password, user.PasswordSalt);
            if (user.UserCode == model.UserCode && user.Password == model.Password)
            {
                _logger.LogInformation($"Authentication succeed for USER: {user.Name}.");
                return Json(user);
            }
            else
            {
                if (null != HttpContext.Session.GetString(SESSION_KEY))
                {
                    if (s_count > 3)
                    {
                        if (LockAccount(model) > 0)
                        {
                            _logger.LogInformation($"Account locked for USER: {user.Name}.");
                            return Json(user);
                        }
                        else
                            return StatusCode(500);
                    }

                    session_count++;
                    _configuration["Session"] = (s_count + session_count).ToString();
                }
            }
            return Unauthorized();
        }

        private int LockAccount(LoginViewModel model)
        {
            // Reset session configuration...
            _configuration["Session"] = "0";

            // Locking account.
            var user = db.Users.Where(u => u.UserCode == model.UserCode).SingleOrDefault();
            user.AccountStatus = AccountStatus.LOCKED;
            db.Users.Update(user);

            return db.SaveChanges();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public IActionResult ChangePassword([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var chkUserStat = db.Users.SingleOrDefault(u => u.Id == user.Id);
            if (chkUserStat.AccountStatus == AccountStatus.INACTIVE)
                return StatusCode(403);

            if (string.IsNullOrEmpty(user.Password)) return BadRequest();
            if (user.Password.Length < 8) return BadRequest();

            if (string.IsNullOrEmpty(user.SecurityQuestion1)) return BadRequest();
            if (string.IsNullOrEmpty(user.SecurityQuestion2)) return BadRequest();
            if (string.IsNullOrEmpty(user.SecurityQuestion3)) return BadRequest();
            if (string.IsNullOrEmpty(user.SecurityQuestion4)) return BadRequest();

            //if (!Regex.IsMatch(user.SecurityQuestion1, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion2, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion3, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion4, "^[a-zA-Z0-9]*$")) return BadRequest();

            var pwdUser = db.Users.Where(
                                u => u.Id == user.Id &&
                                u.UserCode == user.UserCode &&
                                u.PasswordSalt == user.PasswordSalt
                            ).SingleOrDefault();

            if (pwdUser is null) return NotFound();
            pwdUser.Password = service.Encrypt(user.Password, user.PasswordSalt);

            user.SecurityQuestion1 = Regex.Replace(user.SecurityQuestion1, @"\s+", "_");
            user.SecurityQuestion2 = Regex.Replace(user.SecurityQuestion2, @"\s+", "_");
            user.SecurityQuestion3 = Regex.Replace(user.SecurityQuestion3, @"\s+", "_");
            user.SecurityQuestion4 = Regex.Replace(user.SecurityQuestion4, @"\s+", "_");

            pwdUser.SecurityQuestion1 = user.SecurityQuestion1.ToLower();
            pwdUser.SecurityQuestion2 = user.SecurityQuestion2.ToLower();
            pwdUser.SecurityQuestion3 = user.SecurityQuestion3.ToLower();
            pwdUser.SecurityQuestion4 = user.SecurityQuestion4.ToLower();
            pwdUser.IsFirstTimeLogin = false;
            pwdUser.UpdatedDate = DateTime.Now;

            db.Users.Update(pwdUser);

            if (db.SaveChanges() > 0)
            {
                _logger.LogInformation($"Password successfully changed for USER: {user.Name}");
                return Json(pwdUser);
            }
            else
                return Forbid();
        }

        [HttpPost("{code}")]
        [ActionName("CheckUser")]
        public IActionResult CheckUser([FromRoute]string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var user = db.Users.Where(u => u.UserCode == code).SingleOrDefault();
            if (user is null)
                return NotFound();

            ForgotPassword item = new Models.ForgotPassword
            {
                UserCode = user.UserCode,
                SecurityQuestion1 = user.SecurityQuestion1,
                SecurityQuestion2 = user.SecurityQuestion2,
                SecurityQuestion3 = user.SecurityQuestion3,
                SecurityQuestion4 = user.SecurityQuestion4,
                Password = user.Password
            };

            return Json(item);
        }

        [HttpGet("{code}")]
        [ActionName("Reset")]
        public IActionResult Reset([FromRoute]string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var user = db.Users.Where(
                            u => u.UserCode == code &&
                            u.AccountStatus == AccountStatus.ACTIVE &&
                            u.IsFirstTimeLogin == false
                        ).SingleOrDefault();

            if (user is null) return NotFound();
            user.IsFirstTimeLogin = true;
            user.SecurityQuestion1 = null;
            user.SecurityQuestion2 = null;
            user.SecurityQuestion3 = null;
            user.SecurityQuestion4 = null;
            user.UpdatedDate = null;

            db.Users.Update(user);
            if (db.SaveChanges() > 0)
                return Json(user);
            else
                return Forbid();
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody]ForgotPassword forgot)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = db.Users.Where(u => u.UserCode == forgot.UserCode).SingleOrDefault();

            if (user is null) return NotFound();
            if (string.IsNullOrEmpty(forgot.SecurityQuestion1)) return BadRequest();
            if (string.IsNullOrEmpty(forgot.SecurityQuestion2)) return BadRequest();
            if (string.IsNullOrEmpty(forgot.SecurityQuestion3)) return BadRequest();
            if (string.IsNullOrEmpty(forgot.SecurityQuestion4)) return BadRequest();

            //if (!Regex.IsMatch(user.SecurityQuestion1, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion2, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion3, "^[a-zA-Z0-9]*$")) return BadRequest();
            //if (!Regex.IsMatch(user.SecurityQuestion4, "^[a-zA-Z0-9]*$")) return BadRequest();

            if (string.IsNullOrEmpty(forgot.Password)) return BadRequest();
            if (forgot.Password.Length < 8) return BadRequest();

            forgot.SecurityQuestion1 = Regex.Replace(forgot.SecurityQuestion1, @"\s+", "_");
            forgot.SecurityQuestion2 = Regex.Replace(forgot.SecurityQuestion2, @"\s+", "_");
            forgot.SecurityQuestion3 = Regex.Replace(forgot.SecurityQuestion3, @"\s+", "_");
            forgot.SecurityQuestion4 = Regex.Replace(forgot.SecurityQuestion4, @"\s+", "_");

            if (user.SecurityQuestion1.ToLower() == forgot.SecurityQuestion1.ToLower() &&
                user.SecurityQuestion2.ToLower() == forgot.SecurityQuestion2.ToLower() &&
                user.SecurityQuestion3.ToLower() == forgot.SecurityQuestion3.ToLower() &&
                user.SecurityQuestion4.ToLower() == forgot.SecurityQuestion4.ToLower())
            {
                user.Password = service.Encrypt(forgot.Password, user.PasswordSalt);
            }
            else
                return Unauthorized();

            db.Users.Update(user);
            if (db.SaveChanges() > 0)
            {
                _logger.LogInformation($"Reset password successfully for USER: {user.Name}");
                return Json(user);
            }
            else
                return Forbid();
        }
    }
}