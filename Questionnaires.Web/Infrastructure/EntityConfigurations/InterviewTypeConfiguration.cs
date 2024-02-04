using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure.EntityConfigurations;

public class InterviewTypeConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.HasKey(e => e.Id).HasName("interviews_pkey");

        builder.ToTable("interviews");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.NextQuestionId).HasColumnName("next_question_id");
        builder.Property(e => e.SurveyId).HasColumnName("survey_id");

        builder.HasOne(d => d.NextQuestion).WithMany(p => p.Interviews)
            .HasForeignKey(d => d.NextQuestionId)
            .HasConstraintName("interviews_next_question_id_fkey");

        builder.HasOne(d => d.Survey).WithMany(p => p.Interviews)
            .HasForeignKey(d => d.SurveyId)
            .HasConstraintName("interviews_survey_id_fkey");
    }
}