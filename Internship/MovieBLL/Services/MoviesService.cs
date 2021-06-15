using MovieDAL.Repositories;
using MovieDAL.Repositories.Interfaces;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieBLL.Services
{
    public class MoviesService
    {
        private IMoviesRepository MoviesRepository { get; }
        private IGenreRepository GenreRepository { get; }

        private IMovieGenreRepository MovieGenreRepository { get; }
        public MoviesService(IMoviesRepository moviesRepository, IGenreRepository genreRepository, IMovieGenreRepository movieGenreRepository)
        {
            MoviesRepository = moviesRepository;
            GenreRepository = genreRepository;
            MovieGenreRepository = movieGenreRepository;
        }

        public List<MovieModel> GetAllPagination(int pageNumber, string sortParameter = "Name", int pageSize = 5, string direction = "ASC")

        {
            if (pageNumber < 1) pageNumber = 1;
            var totalpages = GetTotalPages();
            if (pageNumber > totalpages) pageNumber = totalpages;
            List<MovieModel> movies = MoviesRepository.GetAllPagination(pageNumber, sortParameter, direction, pageSize);
            foreach (MovieModel m in movies)
            {
                List<int> genderIds = MovieGenreRepository.GetAllGenresPerMovieId(m.MovieId);
                foreach (int id in genderIds) m.GenreList.Add(GenreRepository.GetGendre(id));
            }
            return movies;
        }



        public int GetTotalPages(int pageSize = 5)
        {
            var totalpages = MoviesRepository.GetAll().Count / pageSize + 1;
            return totalpages;
        }

        public void SaveMovie(MovieDto movie)
        {
            List<GenreModel> allGenres = GenreRepository.GetAll();
            int idInsertedGenre = 0;
            int idInsertedMovie = MoviesRepository.Save(movie);

            foreach (string genre in movie.GenreList)
            {
                GenreModel foundGenre = allGenres.Find(g => g.GenreName == genre.ToUpper());
                if (foundGenre == null)
                {
                    idInsertedGenre = GenreRepository.AddGenre(genre);
                    MovieGenreRepository.AddMovieGenre(idInsertedMovie, idInsertedGenre);

                }
                else
                {
                    idInsertedGenre = foundGenre.GenreId;
                    MovieGenreRepository.AddMovieGenre(idInsertedMovie, idInsertedGenre);
                }
            }
        }



        // in edit  can you  add a new genre ?  
        public void UpdateMovie(MovieDto movie)
        {

            List<int> ListIdGenreNewMovie = new List<int>();
            foreach (string s in movie.GenreList)
            {
                var genreFound = GenreRepository.GetAll().Find(g => g.GenreName == s.ToUpper());
                if (genreFound != null)
                {
                    ListIdGenreNewMovie.Add(genreFound.GenreId);
                }
            }
            List<int> ListIdGenreOldMovie = MovieGenreRepository.GetAllGenresPerMovieId(movie.MovieId);
            List<int> DiffAdd = ListIdGenreNewMovie.Except(ListIdGenreOldMovie).ToList();
            List<int> DiffDelete = ListIdGenreOldMovie.Except(ListIdGenreNewMovie).ToList();
            List<GenreModel> AllGenres = GenreRepository.GetAll();

            foreach (int id in DiffAdd)
            {
                if (AllGenres.Find(genre => genre.GenreId == id) == null)
                {
                    throw new Exception("Genre not found");
                }
            }
            foreach (int id in DiffAdd) MovieGenreRepository.AddMovieGenre(movie.MovieId, id);
            foreach (int id in DiffDelete) MovieGenreRepository.DeleteMovieGenreConnection(movie.MovieId, id);
            MoviesRepository.Update(movie);

        }


        public bool DeleteMovie(int movieId)
        {
            return MoviesRepository.Delete(movieId);
        }

    }
}
