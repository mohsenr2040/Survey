using Microsoft.EntityFrameworkCore;
using Staff_Survey.Models.Entities;
using System.Collections.Generic;
using System.Data;

namespace Staff_Survey.Models.Interfaces
{
    public interface IDbContext
    {
        DbSet<Survey> Surveys { get; set; }
        DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<QuestionItems> QuestionItems { get; set; }
        DbSet<UserAnswerItem> UserAnswerItems { get; set; }

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    }
}
