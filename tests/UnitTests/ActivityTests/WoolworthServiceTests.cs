using EverydayRewardsReceipts.Downloader.Domain;
using EverydayRewardsReceipts.Downloader.Domain.AggregateRoots;
using EverydayRewardsReceipts.Downloader.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EverydayRewardsReceipts.Downloader.UnitTests.ActivityTests;

public class WoolworthServiceTests
{
    [Fact]
    public async Task EverydayRewardsAccountTest()
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

        var services = new ServiceCollection()
            .AddDomain(configuration);

        var provider = services.BuildServiceProvider();

        var startDate = new DateOnly(2023, 4, 1);
        var endDate = new DateOnly(2024, 2, 29);

        var sut = provider.GetRequiredService<EverydayRewardsAccount>();

        var tasks = new List<Task>();

        await foreach (var receipt in sut.GetReceipts(startDate, endDate))
        {
            var task = Task.Run(async () =>
            {
                var fileName = $"{receipt.Date:yyyy-MM-dd} Woolworths {receipt.Amount}.pdf";

                using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                using var stream = await sut.Download(receipt);
                
                await stream.CopyToAsync(fileStream);
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }

    [Fact]
    public async Task GetFirstPageTest()
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

        var services = new ServiceCollection()
            .AddDomain(configuration);

        var provider = services.BuildServiceProvider();

        var sut = provider.GetRequiredService<WoolworthService>();

        var activities = await sut.SearchActivitiesAsync(new ActivitySearchRequest());
    }

    [Fact]
    public async Task GetReceiptDetailTest()
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

        var services = new ServiceCollection()
            .AddDomain(configuration);

        var provider = services.BuildServiceProvider();

        var sut = provider.GetRequiredService<WoolworthService>();

        var activities = await sut.GetReceiptDetail(
            new ReceiptDetailGetRequest("U2FsdGVkX19TWJA9ron5dCE4ENeeDuIocaEQTJF5Y8yp2X3uccfNxQE/gOunH/Y1KVwS7qxa7iqvCqkPHv2/YqWXW8DY+M7SSmLjcpOCcrE=")
        );
    }
}
