using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TapFlow.API.Models;

public class Pour
{
    public Guid EventId { get; set; }

    public string DeviceId { get; set; } = string.Empty;

    public string LocationId { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public int VolumeMl { get; set; }
}
