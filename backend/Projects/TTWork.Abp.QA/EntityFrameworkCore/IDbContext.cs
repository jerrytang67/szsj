using Microsoft.EntityFrameworkCore;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.EntityFrameworkCore
{
    // ReSharper disable once InconsistentNaming
    public interface IQADbContext
    {
        public DbSet<QAPlan> QAPlans { get; set; }
        public DbSet<QAQuestion> QAQuestions { get; set; }
        public DbSet<UserQuestionLog> UserQuestionLogs { get; set; }
    }
}