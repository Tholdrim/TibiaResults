using Microsoft.Extensions.DependencyInjection;

namespace TibiaResults.Core
{
    public static class Services
    {
        public static void AddCoreModule(this IServiceCollection services)
        {
            services
                .AddSingleton<IHighscoreRetrievalService, HighscoreRetrievalService>()
                .AddSingleton<ILevelTrackingService, LevelTrackingService>()
                .AddSingleton<IResultComputingService, ResultComputingService>();

            services
                .AddSingleton<IResultFormatter, DiscordFormatter>();
        }
    }
}
