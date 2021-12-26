using ExampleProject.Core.Domain.Common;
using ExampleProject.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ExampleProject.Service
{
    public interface ICommonService
    {
        IQueryable<AppMenu> GetAppMenu();
        IEnumerable<AppMenu> GetUserAppMenu(User user);

        void ExecuteCommand(string sqlCmd);
        DataTable GetData(string sqlCmd);
        DataSet GetDataSet(string sqlCmd);
    }
}
