using MovieLibrary.Dto;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDAL.Repositories.Interfaces
{
    public interface IMovieGenreRepository
    {

        void AddMovieGenre( int idMovie, int idGenre );
        void DeleteMovieGenreConnection(int idMovie , int idGenre);

        List<int> GetAllGenrePerMovieIds(int idMovie);



    }
}
