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
using Newtonsoft.Json.Linq;
using ExampleProject.Core.Domain.Todo;

namespace ExampleProject.Api.Controllers
{
    [Authorize]
    [Route("api/todo")]
    public class TodoController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly ITodoService _todoService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppConfig _appConfig;

        public TodoController(ICommonService commonService,
            IUserService userService,
            ITodoService todoService,
            IHttpContextAccessor contextAccessor,
            IServiceProvider serviceProvider,
            IOptions<AppConfig> appConfig,
            IUnitOfWork uow) : base(uow)
        {
            _commonService = commonService;
            _userService = userService;
            _todoService = todoService;
            _contextAccessor = contextAccessor;
            _serviceProvider = serviceProvider;
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        [Route("todos")]
        public IActionResult GetTodos()
        {
            try
            {
                var todos = (from d in _todoService.GetTodoItems()
                                                    .Include(x => x.CreatorUser)
                            orderby d.CreatedDate descending
                            select new
                            {
                                d.CreatedDate,
                                d.Description,
                                d.IsCompleted,
                                d.Key,
                                CreatorUser = new
                                {
                                    d.CreatorUser.Key,
                                    d.CreatorUser.Username,
                                    d.CreatorUser.DisplayName
                                }
                            })
                            .ToList();

                return Json(todos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("todo/{todoKey}")]
        public IActionResult GetTodo()
        {
            try
            {
                var todoKey = Guid.Parse(RouteData.Values["todoKey"].ToString());

                var todo = (from d in _todoService.GetTodoItems()
                                                    .Include(x => x.CreatorUser)
                             orderby d.CreatedDate descending
                             select new
                             {
                                 d.CreatedDate,
                                 d.Description,
                                 d.IsCompleted,
                                 d.Key,
                                 CreatorUser = new
                                 {
                                     d.CreatorUser.Key,
                                     d.CreatorUser.Username,
                                     d.CreatorUser.DisplayName
                                 }
                             })
                           .FirstOrDefault(x => x.Key == todoKey);

                if (todo == null)
                    throw new Exception("Todo item not found!");

                return Json(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("todo")]
        public IActionResult PostTodo([FromBody] object obj)
        {
            try
            {
                var inData = JObject.Parse(obj.ToString());

                if (inData == null || inData["Description"] == null || inData["IsCompleted"] == null)
                    throw new Exception("Params not found!");

                var description = inData["Description"].Value<string>();
                var isCompleted = inData["IsCompleted"].Value<bool>();

                if (string.IsNullOrEmpty(description))
                    throw new Exception("Please add description!");

                var todo = new TodoItem()
                {
                    Description = description,
                    IsCompleted = isCompleted,
                    CreatorUserKey = CurrentSession.User.Key
                };

                _todoService.InsertTodoItem(todo);
                _uow.SaveChanges();

                return Json(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("todo")]
        public IActionResult PutTodo([FromBody] object obj)
        {
            try
            {
                var inData = JObject.Parse(obj.ToString());
                var todoKey = Guid.Parse(inData["Key"].Value<string>());
                var description = inData["Description"].Value<string>();
                var isCompleted = inData["IsCompleted"].Value<bool>();

                var todo = _todoService.GetTodoItems()
                    .FirstOrDefault(x => x.Key == todoKey);

                if (todo == null)
                    throw new Exception("Todo item not found!");

                todo.Description = description;
                todo.IsCompleted = isCompleted;
                _todoService.UpdateTodoItem(todo, x => x.Description, x => x.IsCompleted);
                _uow.SaveChanges();

                return Json(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("todo/{todoKey}")]
        public IActionResult DeleteTodo()
        {
            try
            {
                var todoKey = Guid.Parse(RouteData.Values["todoKey"].ToString());

                var todo = _todoService.GetTodoItems()
                           .FirstOrDefault(x => x.Key == todoKey);

                if (todo == null)
                    throw new Exception("Todo item not found!");

                _todoService.DeleteTodoItem(todo);
                _uow.SaveChanges();

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
