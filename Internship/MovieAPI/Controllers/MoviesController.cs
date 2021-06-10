﻿using Microsoft.AspNetCore.Mvc;
using MovieBLL.Services;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private MoviesService MoviesService { get; }

        public MoviesController(MoviesService moviesService)
        {
            MoviesService = moviesService;
        }

        /// <summary>
        /// Gets all the movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MovieModel> GetAllMovies()
        {
            return MoviesService.GetAll();
        }

        /// <summary>
        /// Saves a new movie
        /// </summary>
        /// <param name="movie">the movie that needs to be saved</param>
        /// <returns></returns>
        [HttpPost]
        public int SaveMovie(MovieDto movie)
        {
            return MoviesService.SaveMovie(movie);
        }

        /// <summary>
        /// Updates a movie 
        /// </summary>
        /// <param name="movie"></param>
        [HttpPut]
        public void EditMovie(MovieModel movie)
        {
             MoviesService.EditMovie(movie);
        }


        /// <summary>
        /// Deletes a movie 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool DeleteMovie(int id)
        {
            return MoviesService.DeleteMovie(id);
        }
    }
}