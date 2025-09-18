using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TCSTest.DTOs;
using TCSTest.Services;
using TCSTest.Interface;

namespace TCSTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService) => _scheduleService = scheduleService;

        // GET /api/schedule
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _scheduleService.GetAllAsync());

        // GET /api/schedule/channel/{channelId}
        [HttpGet("channel/{channelId:guid}")]
        public async Task<IActionResult> GetByChannel(Guid channelId) => Ok(await _scheduleService.GetByChannelAsync(channelId));

        // GET /api/schedule/now
        [HttpGet("now")]
        public async Task<IActionResult> GetNow() => Ok(await _scheduleService.GetNowPlayingAsync());

        // POST /api/schedule
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleDto dto)
        {
            var created = await _scheduleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByChannel), new { channelId = created.ChannelId }, created);
        }

        // PUT /api/schedule/{channelId}/{contentId}
        [HttpPut("{channelId:guid}/{contentId:guid}")]
        public async Task<IActionResult> Update(Guid channelId, Guid contentId, [FromBody] UpdateScheduleDto dto)
        {
            var updated = await _scheduleService.UpdateAsync(channelId, contentId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE /api/schedule/{channelId}/{contentId}
        [HttpDelete("{channelId:guid}/{contentId:guid}")]
        public async Task<IActionResult> Delete(Guid channelId, Guid contentId)
        {
            var deleted = await _scheduleService.DeleteAsync(channelId, contentId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
