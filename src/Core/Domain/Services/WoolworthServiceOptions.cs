namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public class WoolworthServiceOptions
{
    public required string BaseUrl { get; set; }
    public required string ClientId { get; set; }
    public required string Token { get; set; }
}