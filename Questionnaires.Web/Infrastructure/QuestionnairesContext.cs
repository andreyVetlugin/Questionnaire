using Microsoft.EntityFrameworkCore;
using Questionnaires.Web.Infrastructure.Entities;
using Questionnaires.Web.Infrastructure.EntityConfigurations;

namespace Questionnaires.Web.Infrastructure;

public class QuestionnairesContext : DbContext
{
    public QuestionnairesContext(DbContextOptions<QuestionnairesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Interview> Interviews { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InterviewTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResultTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SurveyTypeConfiguration());
    }
}