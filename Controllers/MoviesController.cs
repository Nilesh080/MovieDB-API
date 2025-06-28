using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBApi_Assignment4.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? year)
        {
            try
            {
                var moviesResponse = year.HasValue
                    ? await _movieService.GetAllAsync(year.Value)
                    : await _movieService.GetAllAsync();

                return Ok(moviesResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var movieResponse = await _movieService.GetByIdAsync(id);
                return Ok(movieResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieRequest request)
        {
            try
            {
                var createdMovie = await _movieService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] MovieRequest request)
        {
            try
            {
                var updatedMovie = await _movieService.UpdateAsync(id, request);
                return Ok(updatedMovie);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _movieService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{movieId}/poster")]
        public async Task<IActionResult> UploadPoster(int movieId, IFormFile posterImage)
        {
            try
            {
                var posterUrl = await _movieService.UploadPosterAsync(movieId, posterImage);
                return Ok(new { posterUrl });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
