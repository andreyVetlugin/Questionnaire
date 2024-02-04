using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure.EntityConfigurations;

public class QuestionTypeConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(e => e.Id).HasName("questions_pkey");

        builder.ToTable("questions");

        builder.HasIndex(e => e.NextQuestionId, "questions_next_question_id_key").IsUnique();

        builder.HasIndex(e => new { e.SurveyId, e.QuestionText }, "survey_question_idx").IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.AllowFewAnswers).HasColumnName("allow_few_answers");
        builder.Property(e => e.NextQuestionId).HasColumnName("next_question_id");
        builder.Property(e => e.QuestionText).HasColumnName("question_text");
        builder.Property(e => e.SurveyId).HasColumnName("survey_id");

        builder.HasOne(d => d.NextQuestion).WithOne(p => p.InverseNextQuestion)
            .HasForeignKey<Question>(d => d.NextQuestionId)
            .HasConstraintName("questions_next_question_id_fkey");

        builder.HasOne(d => d.Survey).WithMany(p => p.Questions)
            .HasForeignKey(d => d.SurveyId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("questions_survey_id_fkey");
    }
}