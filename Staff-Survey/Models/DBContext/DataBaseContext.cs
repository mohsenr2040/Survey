
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Staff_Survey.Models.Entities;
using Staff_Survey.Models.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace Staff_Survey.Models.DataBaseContext
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>, IDbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {


        }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<QuestionItems> QuestionItems { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<UserAnswerItem> UserAnswerItems { get ; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.ProviderKey, p.LoginProvider });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.RoleId, p.UserId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });

            //modelBuilder.Entity<SurveyQuestion>().HasOne(p => p.Survey).WithMany(p => p.SurveyQuestions).OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<QuestionItems>().HasOne(s => s.SurveyQuestion).WithMany(s => s.QuestionItems).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();

            ApplyQueryFilter(modelBuilder);

        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
            modelBuilder.Entity<Survey>().HasQueryFilter(c => !c.IsDeleted );
        }

    }
    
}