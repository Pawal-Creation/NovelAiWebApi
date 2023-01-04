using System;
using Microsoft.Extensions.DependencyInjection;

namespace NovelAi;

public static class NovelAiServiceExtensions
{
    public static T AddNovelAiApi<T>(this T services,IConfiguration config) where T:IServiceCollection
    {
        services.Configure<NovelAiApiOption>(config.GetSection("NovelAi"));
        services.AddSingleton<NovelAiRestfulApi>();
        return services;
    }
}