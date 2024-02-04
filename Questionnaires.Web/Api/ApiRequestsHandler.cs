using System.Linq.Expressions;
using Questionnaires.Web.Infrastructure;
using Questionnaires.Web.Infrastructure.Entities;
using Questionnaires.Web.Views;

namespace Questionnaires.Web.Api;

public static class ApiRequestsHandler
{
    public static async Task<QuestionView?> GetQuestionViewAsync
        (Guid? questionId, QuestionnairesRepository repository) =>
        (await repository.GetMappedQuestionAsync(questionId, MapsProvider.QuestionToQuestionViewMap));

    public static async Task<QuestionView?> GetNextQuestionAsync(
        Guid surveyId,
        QuestionnairesRepository repository,
        HttpContext httpContext)
    {
        var nextQuestionId = await GetNextQuestionIdAsync(surveyId, httpContext, repository);
        return await GetQuestionViewAsync(nextQuestionId, repository);
    }

    public static async Task<IResult> AddResult(
        ResultView result,
        HttpContext httpContext,
        QuestionnairesRepository repository)
    {
        var interviewId = ParseInterviewIdFromCookies(httpContext, result.SurveyId);
        var questionId = await repository.GetNextQuestionIdAsync(result.SurveyId,interviewId);
        var questionWithAnswers = await
            repository.GetMappedQuestionAsync(questionId, MapsProvider.QuestionToQuestionWithAnswersIdsMap);

        if (!await repository.IsSurveyPublic(result.SurveyId))
            return Results.BadRequest("Survey is not yet available");
        
        if (questionWithAnswers is null)
            return Results.NotFound("Question not exist");
        
        if (result.AnswersIds.Count == 0)
            return Results.BadRequest("Select answers");
        
        if (result.AnswersIds.Count > 1 && !questionWithAnswers.Value.Question.AllowFewAnswers)
            return Results.BadRequest("Select only one answer");
        
        if (result.AnswersIds.Except(questionWithAnswers.Value.AnswersIds).Any())
            return Results.BadRequest("Selected answers not attached to question");
        
        if (interviewId is null)
            (interviewId, _) = await repository.CreateInterviewAsync(result.SurveyId, questionWithAnswers.Value.Question.NextQuestionId);
        else
            await repository.ChangeNextQuestionForExistedInterviewAsync(interviewId.Value, questionWithAnswers.Value.Question.NextQuestionId);
        
        await repository.AddQuestionResultsAsync(questionId!.Value, result.AnswersIds, result.SurveyId, interviewId!.Value);
        await repository.AcceptChangesAsync();
        
        SetInterviewIdInCookies(httpContext, result.SurveyId, interviewId!.Value);
        return Results.Ok(questionWithAnswers.Value.Question.NextQuestionId);
    }

    private static async Task<Guid?> GetNextQuestionIdAsync(
        Guid surveyId,
        HttpContext httpContext,
        QuestionnairesRepository repository)
    {
        var interviewId = ParseInterviewIdFromCookies(httpContext, surveyId);
        return await repository.GetNextQuestionIdAsync(surveyId, interviewId);
    }

    private static void SetInterviewIdInCookies(HttpContext httpContext, Guid surveyId, Guid interviewId)
        => httpContext.Response.Cookies.Append($"survey-{surveyId}", interviewId.ToString());

    private static Guid? ParseInterviewIdFromCookies(HttpContext httpContext, Guid surveyId)
        => (httpContext.Request.Cookies.TryGetValue($"survey-{surveyId}", out var interviewIdAsString))
            ? Guid.Parse(interviewIdAsString)
            : null;

    private record struct QuestionWithAnswersIds(Question Question, List<Guid> AnswersIds);

    private static class MapsProvider
    {
        public static readonly Expression<Func<Question, QuestionView>> QuestionToQuestionViewMap =
            question => new QuestionView(
                question.Id,
                question.QuestionText,
                question.AllowFewAnswers,
                question.Answers.Select(x => new AnswerView(x.Id, x.AnswerText)),
                question.NextQuestionId);

        public static readonly Expression<Func<Question, QuestionWithAnswersIds?>> QuestionToQuestionWithAnswersIdsMap
            = (q) =>
                new QuestionWithAnswersIds(q, q.Answers.Select(z => z.Id).ToList());
    }
}