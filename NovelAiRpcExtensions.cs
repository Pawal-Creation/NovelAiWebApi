using System;
using Microsoft.Extensions.DependencyInjection;

namespace NovelAi;

public static class NovelAiRpcExtensions
{
    public static T AddNovelAiRpc<T>(this T services,IConfiguration config) where T:IServiceCollection
    {
        services.Configure<NovelAiRpcOption>(config.GetSection("NovelAi"));
        services.AddSingleton<NovelAiRpc>();
        return services;
    }
}