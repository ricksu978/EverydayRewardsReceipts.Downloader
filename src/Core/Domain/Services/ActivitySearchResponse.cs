namespace EverydayRewardsReceipts.Downloader.Domain.Services;

public record ActivitySearchResponse(ActivitySearchResponse.DataDto Data)
{
    public string NextPageToken => Data.RtlRewardsActivityFeed.List.NextPageToken;
    public IEnumerable<DataDto.RtlRewardsActivityFeedDto.ListDto.GroupDto.ItemDto> Items => Data.RtlRewardsActivityFeed.List.Groups.SelectMany(g => g.Items);

    public record DataDto(DataDto.RtlRewardsActivityFeedDto RtlRewardsActivityFeed)
    {
        public record RtlRewardsActivityFeedDto(RtlRewardsActivityFeedDto.ListDto List)
        {
            public record ListDto(IEnumerable<ListDto.GroupDto> Groups, string NextPageToken)
            {
                public record GroupDto(string Title, IEnumerable<GroupDto.ItemDto> Items)
                {
                    public int Year => Title switch
                    {
                        "This Month" => DateTime.Today.Year,
                        "Last Month" => DateTime.Today.AddMonths(-1).Year,
                        _ => DateOnly.ParseExact(Title, "MMMM yyyy").Year
                    };

                    public record ItemDto(string Id, string DisplayDate, ItemDto.TransactionDto? Transaction, ItemDto.ReceiptDto? Receipt, string TransactionType)
                    {
                        public DateOnly GetDate(int year) => DateOnly.ParseExact($"{DisplayDate} {year}", "ddd dd MMM yyyy");
                        public record TransactionDto(string Origin, string AmountAsDollars);
                        public record ReceiptDto(string ReceiptId);
                    }
                }

            }
        }
    }
}
