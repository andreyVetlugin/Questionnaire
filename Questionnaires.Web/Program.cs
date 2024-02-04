using Questionnaires.Web;
using Questionnaires.Web.Api;
using Questionnaires.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<QuestionnairesRepository>();
builder.AddDbContexts();

var app = builder.Build();

app.MapEndpoints();
app.Run();
