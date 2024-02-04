using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure.EntityConfigurations;

public class SurveyTypeConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.HasKey(e => e.Id).HasName("surveys_pkey");

        builder.ToTable("surveys");

        builder.HasIndex(e => e.FirstQuestionId, "surveys_first_question_id_key").IsUnique();

        builder.HasIndex(e => e.Name, "surveys_name_key").IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.FirstQuestionId).HasColumnName("first_question_id");
        builder.Property(e => e.IsPublic).HasColumnName("is_public");
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.HasOne(d => d.FirstQuestion).WithOne(p => p.SurveyNavigation)
            .HasForeignKey<Survey>(d => d.FirstQuestionId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_surveys_questions");
    }
}