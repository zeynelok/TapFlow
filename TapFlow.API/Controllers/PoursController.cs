using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TapFlow.API.Constants;
using TapFlow.API.Data;
using TapFlow.API.DTOs;
using TapFlow.API.Models;

namespace TapFlow.API.Controllers;

[ApiController]
[Route("v1")]
public class PoursController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public PoursController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("pours")]
    public async Task<IActionResult> RecordPour([FromBody] PourRequest request)
    {
        if (!RuleSet.ValidProducts.Contains(request.ProductId))
            return BadRequest("Invalid ProductId");

        if (!RuleSet.ValidLocations.Contains(request.LocationId))
            return BadRequest("Invalid LocationId");

        if (!RuleSet.ValidVolumes.Contains(request.VolumeMl))
            return BadRequest("Invalid VolumeMl");

        if (request.EndedAt < request.StartedAt)
            return BadRequest("EndedAt must be greater than or equal to StartedAt");

        var existing = await _context.Pours.AnyAsync(p=>p.EventId==request.EventId);
        if (existing)
        {
            return Ok(); 
        }

        var pour = new Pour
        {
            EventId = request.EventId,
            DeviceId = request.DeviceId,
            LocationId = request.LocationId,
            ProductId = request.ProductId,
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            VolumeMl = request.VolumeMl
        };

        _context.Pours.Add(pour);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("taps/{deviceId}/summary")]
    public async Task<IActionResult> GetSummary(string deviceId,[FromQuery] DateTime? from,[FromQuery] DateTime? to)
    {
        var query = _context.Pours.Where(p => p.DeviceId == deviceId);

        if (from.HasValue)
        {
            query = query.Where(p => p.StartedAt >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(p => p.StartedAt <= to.Value);
        }

        var pours = await query.ToListAsync();

        if (!pours.Any())
        {
            return Ok(new TapSummaryResponse
            {
                DeviceId = deviceId,
                From = from,
                To = to,
                TotalVolumeMl = 0,
                TotalPours = 0
            });
        }

        var totalVolume = pours.Sum(p => p.VolumeMl);
        var totalCount = pours.Count;

        var byProduct = pours
            .GroupBy(p => p.ProductId)
            .Select(g => new ProductStatItem
            {
                ProductId = g.Key,
                VolumeMl = g.Sum(p => p.VolumeMl),
                Pours = g.Count()
            })
            .OrderByDescending(x => x.VolumeMl)
            .ToList();

        var byLocation = pours
            .GroupBy(p => p.LocationId)
            .Select(g => new LocationStatItem
            {
                LocationId = g.Key,
                VolumeMl = g.Sum(p => p.VolumeMl),
                Pours = g.Count()
            })
            .OrderByDescending(x => x.VolumeMl)
            .ToList();

        var topProduct = byProduct.FirstOrDefault();
        var topLocation = byLocation.FirstOrDefault();

        var response = new TapSummaryResponse
        {
            DeviceId = deviceId,
            From = from,
            To = to,
            TotalVolumeMl = totalVolume,
            TotalPours = totalCount,
            ByProduct = byProduct,
            ByLocation = byLocation,
            TopProduct = topProduct != null ? new StatItem { ProductId = topProduct.ProductId, VolumeMl = topProduct.VolumeMl, Pours = topProduct.Pours } : null,
            TopLocation = topLocation != null ? new StatItem { LocationId = topLocation.LocationId, VolumeMl = topLocation.VolumeMl, Pours = topLocation.Pours } : null
        };

        return Ok(response);
    }
}
