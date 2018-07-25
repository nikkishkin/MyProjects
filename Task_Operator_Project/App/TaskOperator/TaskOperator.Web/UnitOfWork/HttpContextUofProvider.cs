using TaskOperator.Core;
using TaskOperator.Web.Controllers;

namespace TaskOperator.Web.UnitOfWork
{
    public class HttpContextUofProvider: IUofProvider
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return (IUnitOfWork)System.Web.HttpContext.Current.Items[TaskOperatorController.REQUEST_STORAGE_UNIT_OF_WORK];
        }
    }
}