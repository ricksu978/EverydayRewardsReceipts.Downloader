using EverydayRewardsReceipts.Downloader.Domain;
using EverydayRewardsReceipts.Downloader.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverydayRewardsReceipts.Downloader.UnitTests.ActivityTests;

public class WoolworthServiceTests
{
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
            new ReceiptDetailGetRequest 
            { 
                ReceiptKey = "U2FsdGVkX19TWJA9ron5dCE4ENeeDuIocaEQTJF5Y8yp2X3uccfNxQE/gOunH/Y1KVwS7qxa7iqvCqkPHv2/YqWXW8DY+M7SSmLjcpOCcrE="
            }
        );
    }
}
