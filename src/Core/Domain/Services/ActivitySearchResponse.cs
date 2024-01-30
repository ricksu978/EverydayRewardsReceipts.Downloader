using System.Text.Json.Serialization;
using static EverydayRewardsReceipts.Downloader.Domain.Services.ActivitySearchResponse.DataDto.RtlRewardsActivityFeedDto;

namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public record ActivitySearchResponse(ActivitySearchResponse.DataDto Data)
{
    public record DataDto(DataDto.RtlRewardsActivityFeedDto RtlRewardsActivityFeed)
    {
        public record RtlRewardsActivityFeedDto(ListDto List)
        {
            public record ListDto(IEnumerable<ListDto.GroupDto> Groups, string NextPageToken)
            {
                public record GroupDto(string Title, IEnumerable<GroupDto.ItemDto> Items)
                {
                    public record ItemDto(string Id, string DisplayDate, ItemDto.TransactionDto? Transaction, ItemDto.ReceiptDto? Receipt, string TransactionType)
                    {
                        public record TransactionDto(string Origin, string AmountAsDollars);
                        public record ReceiptDto(string ReceiptId);
                    }
                }

            }
        }
    }
}
