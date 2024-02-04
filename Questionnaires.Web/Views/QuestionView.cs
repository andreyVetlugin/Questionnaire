namespace Questionnaires.Web.Views;

public record QuestionView(Guid Id, string QuestionText, bool AllowFewAnswers, IEnumerable<AnswerView> Answers, Guid? NextQuestionId);
