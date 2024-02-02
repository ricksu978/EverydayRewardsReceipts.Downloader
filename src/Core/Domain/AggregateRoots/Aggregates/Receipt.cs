namespace EverydayRewardsReceipts.Downloader.Domain.AggregateRoots.Aggregates;

public record Receipt(string Id, DateOnly Date, string Amount, string Url);
