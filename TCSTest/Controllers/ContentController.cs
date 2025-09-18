using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCSTest.DTOs;
using TCSTest.Interface;

namespace TCSTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
      private readonly IContentService contentService;
      public ContentController(IContentService contentServiceArg) => contentService = contentServiceArg;

        [HttpGet]
        public async Task<IActionResult> GetAllContent() =>  Ok(await contentService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentWithId(Guid id)
        {
            var item = await contentService.GetAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateContentDto dto)
        {
            var created = await contentService.CreateContentAsync(dto);
            return CreatedAtAction(nameof(GetContentWithId), new { id = created.ContentId }, created);
        }
        

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] UpdateContentDto dto)
        {
            var res = await contentService.UpdateContentAsync(id, dto);
            return res == null ? NotFound() : Ok(res);
        }        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await contentService.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

    }
}
