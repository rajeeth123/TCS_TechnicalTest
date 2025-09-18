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
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;
        public ChannelController(IChannelService channelService) => _channelService = channelService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _channelService.GetAllAsync());
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChannelDto dto)
        {
            var created = await _channelService.CreateAsync(dto);
            return created == null ? NoContent() : Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChannelDto dto)
        {
            var res = await _channelService.UpdateAsync(id, dto);
            return res == null ? NotFound() : Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _channelService.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
