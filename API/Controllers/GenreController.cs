    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MusicCatalog.Api.DTOs;
    using MusicCatalog.Api.DTOs.Requests;
    using MusicCatalog.Api.DTOs.Responses;
    using MusicCatalog.Common.Entities;
    using MusicCatalog.Common.Services;

    namespace MusicCatalog.Api.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            var genres = await _genreService.GetAll();
            var response = genres.Select(GenreResponse.FromEntity);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(GenreResponse.FromEntity(genre));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateGenreRequest createGenreRequest)
        {
            var genre = await _genreService.Create(new Genre { Name = createGenreRequest.Name });
            return Ok(GenreResponse.FromEntity(genre));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, UpdateGenreRequest updateGenreRequest)
        {
            var genre = await _genreService.Update(id, updateGenreRequest.Name);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(GenreResponse.FromEntity(genre));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _genreService.Delete(id);

            if(deleted == false)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }