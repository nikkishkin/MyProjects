using System.Threading;
using TaskOperator.Core;
using TaskOperator.Web.Controllers;

namespace TaskOperator.Web
{
    public class DataProvider: IDataProvider
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return (IUnitOfWork)System.Web.HttpContext.Current.Items[TaskOperatorController.REQUEST_STORAGE_UNIT_OF_WORK];
        }
    }
}