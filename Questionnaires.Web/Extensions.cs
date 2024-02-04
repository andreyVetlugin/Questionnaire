using Microsoft.EntityFrameworkCore;
using Questionnaires.Web.Infrastructure;
using Questionnaires.Web.Infrastructure.Entities;
using Questionnaires.Web.Views;

namespace Questionnaires.Web;

internal static class Extensions
{
    public static void AddDbContexts(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<QuestionnairesContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Questionnaires")));
    }
}