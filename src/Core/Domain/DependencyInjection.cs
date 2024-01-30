using EverydayRewardsReceipts.Downloader.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace EverydayRewardsReceipts.Downloader.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WoolworthService>();
        services.Configure<WoolworthServiceOptions>(configuration.GetSection("WoolworthService"));
        services.AddHttpClient<WoolworthService>((provider, httpClient) =>
        {
            var options = provider.GetRequiredService<IOptions<WoolworthServiceOptions>>();

            httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("client_id", options.Value.ClientId);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.Token);

        });
        
        return services;
    }
}
