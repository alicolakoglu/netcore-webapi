using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ExampleProject.Core.Domain.Common;
using ExampleProject.Core.Domain.Users;
using ExampleProject.Data.UnitOfWork;
using ExampleProject.Service;
using System;
using System.Linq;
using System.Net.Mail;
using static ExampleProject.Api.Utilities;

namespace ExampleProject.Api.Controllers
{
    [Authorize(Policy = "Administrator")]
    [Route("api/manage")]
    public class ManageController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppConfig _appConfig;

        public ManageController(ICommonService commonService,
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IOptions<AppConfig> appConfig,
            IUnitOfWork uow) : base(uow)
        {
            _commonService = commonService;
            _userService = userService;
            _contextAccessor = contextAccessor;
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            try
            {
                var data = (from u in _userService.GetUsers()
                            where !u.IsDeleted
                            select new
                            {
                                u.Key,
                                u.DisplayName,
                                u.Username,
                                u.Email,
                                u.IsAdministrator,
                                u.IsActive,
                            });

                return Json(data
                    .OrderBy(x => x.DisplayName)
                    .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("user")]
        public IActionResult GetUser([FromQuery] Guid key)
        {
            try
            {
                var user = (from u in _userService.GetUsers()
                            where !u.IsDeleted
                            select new
                            {
                                u.Key,
                                u.DisplayName,
                                u.Username,
                                u.Email,
                                u.IsAdministrator,
                                u.IsActive,
                            })
                            .FirstOrDefault(x => x.Key == key);

                if (user == null)
                    throw new Exception("User not found!");

                return Json(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("user")]
        public IActionResult PostUser([FromBody] object obj)
        {
            try
            {
                var inData = JObject.Parse(obj.ToString());
                var displayName = inData["DisplayName"].Value<string>();
                var email = inData["Email"].Value<string>();
                var username = inData["Username"].Value<string>();
                var isAdministrator = inData["IsAdministrator"].Value<bool>();

                if (!string.IsNullOrEmpty(email) && !Utilities.IsValidEmail(email))
                    throw new Exception("Email address is not valid!");

                if (username.Length < 5 || username.Length > 20)
                    throw new Exception("Username min. 5 max. 20 character!");

                var user = _userService.GetUsers()
                    .FirstOrDefault(x => !x.IsDeleted && x.Username == username);

                if (user != null)
                    throw new Exception("Username already taken!");

                var passw = new Random().Next(100000, 999999).ToString();

                var passwMD5 = Utilities.ConvertStringToMD5(passw);

                var nUser = new User()
                {
                    IsAdministrator = isAdministrator,
                    IsActive = true,
                    IsDeleted = false,
                    DisplayName = displayName,
                    Username = username,
                    Email = email,
                    Password = passwMD5
                };
                _userService.InsertUser(nUser);

                /// Send user password to user mail... ///

                _uow.SaveChanges();
                return Json(nUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("user")]
        public IActionResult PutUser([FromBody]object obj)
        {
            try
            {
                var inData = JObject.Parse(obj.ToString());
                var key = Guid.Parse(inData["Key"].Value<string>());
                var displayName = inData["DisplayName"].Value<string>();
                var email = inData["Email"].Value<string>();
                var isAdministrator = inData["IsAdministrator"].Value<bool>();
                var isActive = inData["IsActive"].Value<bool>();

                if (!IsValidEmail(email))
                    throw new Exception("Email address is not valid!");

                var user = _userService.GetUsers()
                    .FirstOrDefault(x => x.Key == key);

                if (user == null)
                    throw new Exception("User not found!");

                if (user.Email != email && _userService.GetUsers().Any(x => !x.IsDeleted && x.Key != key && x.Email == email))
                    throw new Exception("Email address already taken!");

                user.DisplayName = displayName;
                user.Email = email;
                user.IsAdministrator = isAdministrator;
                user.IsActive = isActive;
                user.IsDeleted = false;

                _userService.UpdateUser(user, x => x.DisplayName, x => x.Email, x => x.IsAdministrator, x => x.IsActive, x => x.IsDeleted);
                _uow.SaveChanges();

                return Json(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("user/{key}")]
        public IActionResult DeleteUser()
        {
            try
            {
                var key = Guid.Parse(RouteData.Values["key"].ToString());

                var user = _userService.GetUsers()
                    .FirstOrDefault(x => x.Key == key);

                if (user != null)
                {
                    user.IsDeleted = true;
                    _userService.UpdateUser(user, x => x.IsDeleted);
                    _uow.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _uow.Dispose();
            base.Dispose(disposing);
        }
    }
}