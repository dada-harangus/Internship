using MovieDAL.Repositories;
using MovieDAL.Repositories.Interfaces;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieBLL.Services
{
    public class MoviesService
    {
        private IMoviesRepository MoviesRepository { get; }
        private IGenreRepository GenreRepository { get; }

        private IMovieGenreRepository MovieGenreRepository { get; }
        public MoviesService(IMoviesRepository moviesRepository , IGenreRepository genreRepository ,IMovieGenreRepository movieGenreRepository)
        {
            MoviesRepository = moviesRepository;
            GenreRepository = genreRepository;
            MovieGenreRepository = movieGenreRepository;
        }
        
        public List<MovieModel> GetAll()
        {
            List<MovieModel> movies = MoviesRepository.GetAll();
            foreach (MovieModel m in movies)
            {
               m.GenreList  = GenreRepository.GetAllGenrePerMovie(m.MovieId);
            }
            return movies;
        }
        //add later what if are more then one new genre in list
        public void SaveMovie(MovieDto movie)
        {
            List<GenreModel> allGenres = GenreRepository.GetAll();
            int idInsertedGenre = 0;
            foreach (GenreModel genre in movie.GenreList)
            {
                GenreModel foundGenre = allGenres.Find(g => g.GenreName == genre.GenreName);
                if (foundGenre == null)
                {
                    idInsertedGenre = GenreRepository.AddGenre(genre);
                    
                }
            }
            int idInsertedMovie = MoviesRepository.Save(movie);
           
            foreach (GenreModel g in movie.GenreList) MovieGenreRepository.AddMovieGenre(idInsertedMovie, idInsertedGenre);

        }
        //needs testing
        public void EditMovie(MovieModel movie)
        {
           
            List<int> ListIdGenreNewMovie = MovieGenreRepository.GetAllGenrePerMovieIds(movie.MovieId);
            List<int> ListIdGenreOldMovie = movie.GenreList.Select(x => x.GenreId).ToList();
            List<int> DiffAdd = ListIdGenreNewMovie.Except(ListIdGenreOldMovie).ToList();
            List<int> DiffDelete = ListIdGenreOldMovie.Except(ListIdGenreNewMovie).ToList();
            foreach (int id in DiffAdd) MovieGenreRepository.AddMovieGenre(movie.MovieId, id);
            foreach (int id in DiffDelete) MovieGenreRepository.DeleteMovieGenreConnection(movie.MovieId, id);
            MoviesRepository.Edit(movie);
            
        }


        public bool DeleteMovie(int id)
        {
            return MoviesRepository.Delete(id);
        }

    }
}
