using System.Net.Http.Json;

namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public class WoolworthService
{
    private readonly HttpClient _httpClient;

    public WoolworthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ActivitySearchResponse> SearchActivitiesAsync(ActivitySearchRequest request, CancellationToken token = default)
    {
        var query = "query { rtlRewardsActivityFeed(pageToken: \"" + request.PageToken + "\") { list { groups { ...on RewardsActivityFeedGroup { title items { id displayDate transaction { origin amountAsDollars } receipt { receiptId } transactionType  } } }  nextPageToken } } }";

        var data = JsonContent.Create(new { query });

        var response = await _httpClient.PostAsync("/wx/v1/bff/graphql", data, token);

        return await response.Content.ReadFromJsonAsync<ActivitySearchResponse>(token)
            ?? throw new Exception("Failed to deserialize response");
    }

    public async Task<ReceiptDetailGetResponse> GetReceiptDetail(ReceiptDetailGetRequest request, CancellationToken token = default)
    {
        var data = JsonContent.Create(request);

        var response = await _httpClient.PostAsync("/wx/v1/bff/ereceipts/details", data, token);

        return await response.Content.ReadFromJsonAsync<ReceiptDetailGetResponse>(token)
            ?? throw new Exception("Failed to deserialize response");
    }

    public async Task<ReceiptDetailDownloadResponse> DownloadReceiptDetail(ReceiptDetailDownloadRequest request, CancellationToken token = default)
    {
        var data = JsonContent.Create(request);

        var response = await _httpClient.PostAsync("/wx/v1/bff/ereceipts/details/download", data, token);

        var stream = await response.Content.ReadAsStreamAsync(token);

        return new ReceiptDetailDownloadResponse(stream);
    }
}
