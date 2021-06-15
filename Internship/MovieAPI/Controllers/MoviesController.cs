using Microsoft.AspNetCore.Mvc;
using MovieBLL.Services;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieAPI.Controllers
{

    /// <summary>
    /// Controller for movie entity 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {
        private MoviesService MoviesService { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moviesService"></param>
        public MoviesController(MoviesService moviesService)
        {
            MoviesService = moviesService;
        }

        /// <summary>
        /// Gets all the movies paginated
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public List<MovieModel> GetAllPagination(int pageNumber, string sortParameter = "Name", int pageSize = 5, string direction = "ASC")
        {
            return MoviesService.GetAllPagination(pageNumber, sortParameter, pageSize, direction);
        }
        /// <summary>
        /// Gets the total numbers of pages needed 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetTotalPages()
        {
            return MoviesService.GetTotalPages();
        }

        /// <summary>
        /// Saves a new movie
        /// </summary>
        /// <param name="movie">the movie that needs to be saved</param>
        /// <returns></returns>
        [HttpPost]
        public void SaveMovie(MovieDto movie)
        {
            MoviesService.SaveMovie(movie);
        }

        /// <summary>
        /// Updates a movie 
        /// </summary>
        /// <param name="movie"></param>
        [HttpPut]
        public void UpdateMovie(MovieDto movie)
        {
            MoviesService.UpdateMovie(movie);
        }


        /// <summary>
        /// Deletes a movie 
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool DeleteMovie(int movieId)
        {
            return MoviesService.DeleteMovie(movieId);
        }
    }
}
