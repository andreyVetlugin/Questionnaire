using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaires.Web.Infrastructure.Entities;

public partial class Question
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? SurveyId { get; set; }

    public Guid? NextQuestionId { get; set; }

    public string QuestionText { get; set; }

    public bool AllowFewAnswers { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    public virtual Question? InverseNextQuestion { get; set; }

    public virtual Question? NextQuestion { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Survey? Survey { get; set; }

    public virtual Survey? SurveyNavigation { get; set; }
}
