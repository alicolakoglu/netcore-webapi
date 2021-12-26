using Microsoft.AspNetCore.Mvc;
using ExampleProject.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExampleProject.Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork _uow;

        protected BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
