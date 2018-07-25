namespace TaskOperator.Core
{
    public interface IUofProvider
    {
        IUnitOfWork GetUnitOfWork();
    }
}
