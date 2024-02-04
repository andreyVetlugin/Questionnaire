using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaires.Web.Infrastructure.Entities;

public partial class Result
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? InterviewId { get; set; }

    public Guid? AnswerId { get; set; }

    public Guid? QuestionId { get; set; }

    public virtual Answer? Answer { get; set; }

    public virtual Interview? Interview { get; set; }

    public virtual Question? Question { get; set; }
}
