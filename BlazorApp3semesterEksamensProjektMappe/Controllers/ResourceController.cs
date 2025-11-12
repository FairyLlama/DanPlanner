using BlazorApp3semesterEksamensProjektMappe.Services;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp3semesterEksamensProjektMappe.Controllers
{
    [ApiController]
    [Route("api/resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceDataService _data;
        public ResourceController(IResourceDataService data) => _data = data;

        [HttpGet]
        public async Task<ActionResult<List<ResourceDto>>> GetAll() => await _data.GetAllAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResourceDto>> GetById(int id)
        {
            var r = await _data.GetByIdAsync(id);
            return r is null ? NotFound() : r;
        }
    }

}
