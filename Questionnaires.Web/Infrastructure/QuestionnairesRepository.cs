using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Questionnaires.Web.Infrastructure.Entities;

namespace Questionnaires.Web.Infrastructure;

public class QuestionnairesRepository
{
    private readonly QuestionnairesContext _questionnairesContext;

    public QuestionnairesRepository(QuestionnairesContext questionnairesContext)
    {
        _questionnairesContext = questionnairesContext;
    }

    public async Task<bool> IsSurveyPublic(Guid surveyId)
        => await _questionnairesContext.Surveys.Where(x => x.Id == surveyId).Select(x => x.IsPublic).FirstAsync();

    public async Task<T?> GetMappedQuestionAsync<T>(Guid? questionId, Expression<Func<Question, T>> map) =>
        await _questionnairesContext.Questions
            .Where(x => x.Id == questionId)
            .Select(map)
            .FirstOrDefaultAsync();

    public async Task<Guid?> GetNextQuestionIdAsync(Guid? surveyId, Guid? interviewId = null)
        => interviewId is null
            ? await GetFirstSurveyQuestionAsync(surveyId)
            : await GetNextInterviewQuestionAsync(interviewId.Value);

    public async Task<(Guid InterviewId, Guid? FirstInterviewQuestionId)> CreateInterviewAsync(Guid surveyId, Guid? startQuestion)
    {
        var interview = new Interview()
            { Id = Guid.NewGuid(), SurveyId = surveyId, NextQuestionId = startQuestion };
        await _questionnairesContext.Interviews.AddAsync(interview);
        return (interview.Id, interview.NextQuestionId);
    }

    public async Task AddQuestionResultsAsync(
        Guid questionId,
        IEnumerable<Guid> answersIds,
        Guid surveyId,
        Guid interviewId)
    {
        await _questionnairesContext.Results.AddRangeAsync(answersIds
            .Select(x => new Result { AnswerId = x, InterviewId = interviewId, QuestionId = questionId }));
    }

    public async Task ChangeNextQuestionForExistedInterviewAsync(Guid interviewId, Guid? nextQuestionId)
    {
        await _questionnairesContext.Interviews.Where(x => x.Id == interviewId)
            .ExecuteUpdateAsync(b => b.SetProperty(x => x.NextQuestionId, nextQuestionId));
    }

    public async Task<Guid?> GetNextInterviewQuestionAsync(Guid interviewId)
        => await _questionnairesContext.Interviews
            .Where(x => x.Id == interviewId)
            .Select(x => x.NextQuestionId)
            .FirstAsync();

    public async Task<Guid?> GetFirstSurveyQuestionAsync(Guid? surveyId) =>
        await _questionnairesContext.Surveys
            .Where(x => x.Id == surveyId)
            .Select(x => x.FirstQuestionId)
            .FirstOrDefaultAsync();

    public async Task AcceptChangesAsync() => await _questionnairesContext.SaveChangesAsync();
}