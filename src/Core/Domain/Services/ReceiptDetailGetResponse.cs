namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public record ReceiptDetailGetResponse(ReceiptDetailGetResponse.DataDto Data)
{
    public record DataDto(DataDto.ReceiptDetailsDto ReceiptDetails)
    {
        public record ReceiptDetailsDto(ReceiptDetailsDto.DownloadDto Download, IEnumerable<ReceiptDetailsDto.DetailDto> Details)
        {
            public record DownloadDto(string Url, string Filename);

            public record DetailDto();
        }
    }
}