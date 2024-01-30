using EverydayRewardsReceipts.Downloader.Domain.Services;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text.Json;
using static EverydayRewardsReceipts.Downloader.Domain.Services.ActivitySearchResponse.DataDto.RtlRewardsActivityFeedDto.ListDto.GroupDto;
using static EverydayRewardsReceipts.Downloader.Domain.Services.ActivitySearchResponse.DataDto.RtlRewardsActivityFeedDto.ListDto.GroupDto.ItemDto;

namespace UnitTests.ActivityTests;

public class ActivityTests
{
    [Fact]
    public void ActivityResponseTest()
    {
        // Arange
        var jsonString = File.ReadAllText("ActivityTests/JsonData/ActivityResponse.json");

        // Act
        //var sut = JsonConvert.DeserializeObject<ActivityResponse>(jsonString);
        var sut = Deserialize<ActivitySearchResponse>(jsonString);

        // Assert
        sut.Should().NotBeNull();
    }

    [Fact]
    public void ItemDtoTest()
    {
        // Arange
        var jsonString = File.ReadAllText("ActivityTests/JsonData/ItemDto.json");
        var expectedReceipt = new ReceiptDto("U2FsdGVkX1+L7ROlt6alHT/fM1ZdCwL+EV21CawaMvibYE0ukLxAAxeO2AmTWQtjzET89y0NheRln45eeVK1UuGovfcq2tkrbWXztpbGvuQ=");
        var expectedTransaction = new TransactionDto("Carlingford", "$36.69");

        // Act
        //var sut = JsonConvert.DeserializeObject<ItemDto>(jsonString);
        var sut = Deserialize<ItemDto>(jsonString);

        // Assert
        sut.Should().NotBeNull();
        sut!.Id.Should().Be("1659014271");
        sut!.TransactionType.Should().Be("purchase");
        AssertReceiptDto(sut.Receipt!, expectedReceipt);
        AssertTransactionDto(sut.Transaction!, expectedTransaction);
    }

    [Fact]
    public void TransactionDtoTest()
    {
        // Arange
        var jsonString = File.ReadAllText("ActivityTests/JsonData/TransactionDto.json");
        var expected = new TransactionDto("Carlingford", "$36.69");

        // Act
        var sut = Deserialize<TransactionDto>(jsonString);

        // Assert
        AssertTransactionDto(sut!, expected);
    }

    [Fact]
    public void ReceiptDtoTest()
    {
        // Arange
        var jsonString = File.ReadAllText("ActivityTests/JsonData/ReceiptDto.json");
        var expected = new ReceiptDto("U2FsdGVkX1+L7ROlt6alHT/fM1ZdCwL+EV21CawaMvibYE0ukLxAAxeO2AmTWQtjzET89y0NheRln45eeVK1UuGovfcq2tkrbWXztpbGvuQ=");

        // Act
        var sut = Deserialize<ReceiptDto>(jsonString);

        // Assert
        AssertReceiptDto(sut!, expected);
    }

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private static T? Deserialize<T>(string jsonString)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(
            jsonString,
            _options
        );
    }

    private static void AssertReceiptDto(ReceiptDto sut, ReceiptDto expected)
    {
        sut.Should().NotBeNull();
        sut!.ReceiptId.Should().Be(expected.ReceiptId);
    }

    private static void AssertTransactionDto(TransactionDto sut, TransactionDto expected)
    {
        sut.Should().NotBeNull();
        sut!.Origin.Should().Be(expected.Origin);
        sut!.AmountAsDollars.Should().Be(expected.AmountAsDollars);
    }


}