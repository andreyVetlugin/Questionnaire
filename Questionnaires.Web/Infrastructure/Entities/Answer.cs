using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaires.Web.Infrastructure.Entities;

public partial class Answer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
