using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure.EntityConfigurations;

public class ResultTypeConfiguration : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder.HasKey(e => e.Id).HasName("results_pkey");

        builder.ToTable("results");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.AnswerId).HasColumnName("answer_id");
        builder.Property(e => e.InterviewId).HasColumnName("interview_id");
        builder.Property(e => e.QuestionId).HasColumnName("question_id");

        builder.HasOne(d => d.Answer).WithMany(p => p.Results)
            .HasForeignKey(d => d.AnswerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("results_answer_id_fkey");

        builder.HasOne(d => d.Interview).WithMany(p => p.Results)
            .HasForeignKey(d => d.InterviewId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("results_interview_id_fkey");

        builder.HasOne(d => d.Question).WithMany(p => p.Results)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("results_question_id_fkey");
    }
}