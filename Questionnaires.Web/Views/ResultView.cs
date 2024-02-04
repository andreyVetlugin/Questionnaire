namespace Questionnaires.Web.Views;

public record ResultView(Guid SurveyId, List<Guid> AnswersIds);