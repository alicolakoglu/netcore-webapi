using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ExampleProject.Data.UnitOfWork;
using ExampleProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleProject.Api.Controllers
{
    [Authorize]
    [Route("api/common")]
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppConfig _appConfig;

        public CommonController(ICommonService commonService,
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IServiceProvider serviceProvider,
            IOptions<AppConfig> appConfig,
            IUnitOfWork uow) : base(uow)
        {
            _commonService = commonService;
            _userService = userService;
            _contextAccessor = contextAccessor;
            _serviceProvider = serviceProvider;
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        [Route("app-menu")]
        public IActionResult GetAppMenu()
        {
            try
            {
                var menus = _commonService.GetAppMenu()
                    .Where(x => x.IsActive)
                    .ToList();

                var appMenu = (from m in menus
                               where m.ParentId == null
                               select new
                               {
                                   key = m.ID,
                                   title = m.Title,
                                   href = m.Href,
                                   icon = m.Icon,
                                   children = menus.Any(x => x.ParentId == m.ID) ?
                                   (from mp in menus
                                    where mp.ParentId == m.ID
                                    select new
                                    {
                                        title = mp.Title,
                                        href = mp.Href,
                                    }) : null
                               })
                       .ToList();

                return Json(appMenu);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error! {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _uow.Dispose();
            base.Dispose(disposing);
        }
    }
}
