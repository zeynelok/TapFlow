namespace TapFlow.API.DTOs;

public class TapSummaryResponse
{
    public string DeviceId { get; set; } = string.Empty;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int TotalVolumeMl { get; set; }
    public int TotalPours { get; set; }
    public StatItem? TopProduct { get; set; }
    public StatItem? TopLocation { get; set; }
    public List<ProductStatItem> ByProduct { get; set; } = new();
    public List<LocationStatItem> ByLocation { get; set; } = new();
}

public class StatItem
{
    public string ProductId { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
    public int VolumeMl { get; set; }
    public int Pours { get; set; }
}

public class ProductStatItem
{
    public string ProductId { get; set; } = string.Empty;
    public int VolumeMl { get; set; }
    public int Pours { get; set; }
}

public class LocationStatItem
{
    public string LocationId { get; set; } = string.Empty;
    public int VolumeMl { get; set; }
    public int Pours { get; set; }
}
