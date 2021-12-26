using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ExampleProject.Data.UnitOfWork;
using ExampleProject.Service;
using System;
using System.Linq;

namespace ExampleProject.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppConfig _appConfig;

        public AccountController(
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IOptions<AppConfig> appConfig,
            IUnitOfWork uow) : base(uow)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _appConfig = appConfig.Value;
        }

        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginInfo loginInfo)
        {
            try
            {
                string username = loginInfo.Username;
                string password = loginInfo.Password;

                var user = _userService.GetUsers()
                    .FirstOrDefault(x => (x.Username == username) && !x.IsDeleted && x.IsActive);

                if (user == null)
                    throw new Exception("Login failed!");

                if (user.Password != Utilities.ConvertStringToMD5(password))
                    throw new Exception("Username or password incorrect!");

                return Json(Helpers.JwtHelper.GenerateToken(user));
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
