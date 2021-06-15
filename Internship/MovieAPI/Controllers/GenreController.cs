using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBLL.Services;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{ /// <summary>
  /// Controller for genre entity 
  /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private GenreService GenreService { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genreService"></param>
        public GenreController(GenreService genreService)
        {
            GenreService = genreService;
        }

        /// <summary>
        /// Gets all the genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<GenreModel> GetAllGenres()
        {
            return GenreService.GetAll();
        }

    }
}
