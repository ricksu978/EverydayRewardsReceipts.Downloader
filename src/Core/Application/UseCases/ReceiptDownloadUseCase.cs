using EverydayRewardsReceipts.Downloader.Domain.Services;

namespace EverydayRewardsReceipts.Downloader.Application.UseCases;

public record ReceiptDownloadRequest(DateOnly From, DateOnly? To);

public class ReceiptDownloadUseCase
{
    private readonly WoolworthService _woolworthService;

    public ReceiptDownloadUseCase(WoolworthService woolworthService)
    {
        _woolworthService = woolworthService;
    }

    public async Task<ReceiptDetailDownloadResponse> ExecuteAsync(ReceiptDownloadRequest request, CancellationToken token)
    {
        


        return new ReceiptDetailDownloadResponse(stream);
    }
}
