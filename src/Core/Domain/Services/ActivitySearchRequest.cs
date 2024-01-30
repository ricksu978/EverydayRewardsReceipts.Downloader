namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public record ActivitySearchRequest(string PageToken = ActivitySearchRequest.FIRST_PAGE)
{
    internal const string FIRST_PAGE = "FIRST_PAGE";
}
