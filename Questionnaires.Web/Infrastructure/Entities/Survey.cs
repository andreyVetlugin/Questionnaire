using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaires.Web.Infrastructure.Entities;

public partial class Survey
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? FirstQuestionId { get; set; }

    public bool IsPublic { get; set; }

    public virtual Question? FirstQuestion { get; set; }

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
