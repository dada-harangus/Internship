using MovieDAL.Repositories;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieBLL.Services
{
    public class MoviesService
    {
        private IMoviesRepository MoviesRepository { get; }

        public MoviesService(IMoviesRepository moviesRepository)
        {
            MoviesRepository = moviesRepository;
        }

        public List<MovieModel> GetAll()
        {
            return MoviesRepository.GetAll();
        }

        public int SaveMovie (MovieDto movie)
        {
            return MoviesRepository.Save(movie);
        }

        public void EditMovie (MovieModel movie)
        {
             MoviesRepository.Edit(movie);
        }


        public bool DeleteMovie (int id )
        {
            return MoviesRepository.Delete(id);
        }

    }
}
