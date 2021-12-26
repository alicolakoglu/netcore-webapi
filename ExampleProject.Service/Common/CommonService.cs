using ExampleProject.Core.Domain.Common;
using ExampleProject.Core.Domain.Users;
using ExampleProject.Data.Repository;
using ExampleProject.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ExampleProject.Service
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<AppMenu> _appMenuRepository;
        private readonly IUserService _userService;

        public CommonService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _appMenuRepository = uow.GetRepository<AppMenu>();

            _userService = userService;
        }

        public IQueryable<AppMenu> GetAppMenu()
        {
            return _appMenuRepository.GetAll();
        }
        public IEnumerable<AppMenu> GetUserAppMenu(User user)
        {
            return _appMenuRepository.GetAll();
        }


        public void ExecuteCommand(string sqlCmd)
        {
            _appMenuRepository.ExecuteCommand(sqlCmd);
        }

        public DataTable GetData(string sqlCmd)
        {
            return _appMenuRepository.GetData(sqlCmd);
        }

        public DataSet GetDataSet(string sqlCmd)
        {
            return _appMenuRepository.GetDataSet(sqlCmd);
        }
    }
}
