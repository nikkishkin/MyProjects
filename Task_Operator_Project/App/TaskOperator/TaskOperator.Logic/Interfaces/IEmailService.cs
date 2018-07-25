using TaskOperator.Entities;

namespace TaskOperator.Logic.Interfaces
{
    public interface IEmailService
    {
        void CheckEmailSending(Task oldTask, int newPercentage);
        void CheckEmailSending(Task oldTask, Task newTask);
    }
}