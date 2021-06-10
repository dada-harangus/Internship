using MovieDAL.Repositories.Interfaces;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBLL.Services
{
   public class GenreService
    {
        private IGenreRepository GenreRepository { get; }

        public GenreService(IGenreRepository genreRepository)
        {
            GenreRepository = genreRepository;
        }

        public List<GenreModel> GetAll()
        {
            return GenreRepository.GetAll();
        }


    }
}
