using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaires.Web.Infrastructure.Entities;

public partial class Interview
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid SurveyId { get; set; }

    public Guid? NextQuestionId { get; set; }

    public virtual Question? NextQuestion { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Survey Survey { get; set; } = null!;
}
