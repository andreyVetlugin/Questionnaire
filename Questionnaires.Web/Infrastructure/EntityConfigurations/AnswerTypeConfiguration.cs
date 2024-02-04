using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure.EntityConfigurations;

public class AnswerTypeConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(e => e.Id).HasName("answers_pkey");

        builder.ToTable("answers");

        builder.HasIndex(e => new { e.AnswerText, e.QuestionId }, "answer_question_idx").IsUnique();

        builder.HasIndex(e => e.QuestionId, "question_idx");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.AnswerText).HasColumnName("answer_text");
        builder.Property(e => e.QuestionId).HasColumnName("question_id");

        builder.HasOne(d => d.Question).WithMany(p => p.Answers)
            .HasForeignKey(d => d.QuestionId)
            .HasConstraintName("answers_question_id_fkey");
    }
}