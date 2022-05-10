using Microsoft.Extensions.DependencyInjection;

namespace TibiaResults.Formatters
{
    public static class Services
    {
        public static void AddFormatters(this IServiceCollection services)
        {
            services
                .AddSingleton<IResultTokenizer, ResultTokenizer>();

            services
                .AddSingleton<IResultFormatter, DiscordFormatter>()
                .AddSingleton<IResultFormatter, HtmlFormatter>()
                .AddSingleton<IResultFormatter, PlainTextFormatter>();
        }
    }
}
