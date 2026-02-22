using System.ComponentModel.DataAnnotations;

namespace TapFlow.API.DTOs;

public class PourRequest
{
    [Required]
    public Guid EventId { get; set; }

    [Required]
    public string DeviceId { get; set; } = string.Empty;

    [Required]
    public string LocationId { get; set; } = string.Empty;

    [Required]
    public string ProductId { get; set; } = string.Empty;

    [Required]
    public DateTime StartedAt { get; set; }

    [Required]
    public DateTime EndedAt { get; set; }

    [Required]
    public int VolumeMl { get; set; }
}
