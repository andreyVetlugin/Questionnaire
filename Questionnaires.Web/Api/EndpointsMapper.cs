namespace Questionnaires.Web.Api;

public static class EndpointsMapper
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/questions/{questionId}", ApiRequestsHandler.GetQuestionViewAsync);
        app.MapGet("/questions/next", ApiRequestsHandler.GetNextQuestionAsync);

        app.MapPost("/results", ApiRequestsHandler.AddResult);
        return app;
    }
}