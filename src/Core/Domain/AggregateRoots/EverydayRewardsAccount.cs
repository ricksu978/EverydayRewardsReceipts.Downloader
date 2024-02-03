using EverydayRewardsReceipts.Downloader.Domain.AggregateRoots.Aggregates;
using EverydayRewardsReceipts.Downloader.Domain.Services;

namespace EverydayRewardsReceipts.Downloader.Domain.AggregateRoots;

public class EverydayRewardsAccount
{
    private readonly WoolworthService _woolworthService;

    public EverydayRewardsAccount(WoolworthService woolworthService)
    {
        _woolworthService = woolworthService;
    }

    public async IAsyncEnumerable<Receipt> GetReceipts(DateOnly startDate, DateOnly endDate)
    {
        var searchRequest = new ActivitySearchRequest();
        var doContinue = true;
        ActivitySearchResponse searchResponse;

        do
        {
            searchResponse = await _woolworthService.SearchActivitiesAsync(searchRequest);

            if (searchResponse?.Data == null)
                break;

            foreach (var group in searchResponse.Data.RtlRewardsActivityFeed.List.Groups)
            {
                foreach (var item in group.Items)
                {
                    var date = item.GetDate(group.Year);

                    if (date > endDate || item.Transaction == null || item.Receipt == null)
                    {
                        continue;
                    }

                    if (date < startDate)
                    {
                        yield break;
                    }

                    var response = await _woolworthService.GetReceiptDetail(
                        new ReceiptDetailGetRequest(item.Receipt.ReceiptId)
                    );
                    
                    yield return new Receipt(
                        item.Id,
                        date,
                        item.Transaction.AmountAsDollars,
                        response.Data.ReceiptDetails.Download.Url
                    );
                }
            }

            searchRequest = new ActivitySearchRequest(searchResponse.NextPageToken);
        }
        while (doContinue);
    }

    public async Task<Stream> Download(Receipt receipt)
    {
        var request = new ReceiptDetailDownloadRequest(receipt.Url);

        return (await _woolworthService.DownloadReceiptDetail(request)).Stream;
    }
}
