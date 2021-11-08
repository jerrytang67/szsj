using System.Collections.Generic;
using Abp.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.EntityFrameworkCore
{
    // ReSharper disable once InconsistentNaming
    public static class QADbExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static void ConfigurQA(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QAPlan>(b =>
            {
                b.ToTable(QAConsts.DbTablePrefix + "QAPlans", QAConsts.DbSchema);
                b.Property(x => x.Settings).HasConversion(
                    v => v.ToString(),
                    v => JObject.Parse(v)
                );

                b.Property(x => x.PointRules).HasConversion(
                    v => v.ToJsonString(false, false),
                    v => v.FromJsonString<List<PointRule>>()
                );
            });

            modelBuilder.Entity<QAQuestion>(b =>
            {
                b.ToTable(QAConsts.DbTablePrefix + "QAQuestions", QAConsts.DbSchema);
                b.Property(x => x.AnswerList).HasConversion(
                    v => v.ToJsonString(false, false),
                    v => v.FromJsonString<List<string>>()
                );
            });

            modelBuilder.Entity<UserQuestionLog>(b =>
            {
                b.ToTable(QAConsts.DbTablePrefix + "UserQuestionLogs", QAConsts.DbSchema);
                b.Property(x => x.QuestionItems).HasConversion(
                    v => v.ToJsonString(false, false),
                    v => v.FromJsonString<List<QuestionItem>>()
                );
            });
        }
    }
}