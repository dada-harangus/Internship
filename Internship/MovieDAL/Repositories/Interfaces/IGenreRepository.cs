using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDAL.Repositories.Interfaces
{
   public interface IGenreRepository
    {
        public List<GenreModel> GetAll();
        
        int AddGenre(GenreModel foundGenre);
        List<GenreModel> GetAllGenrePerMovie(int idMovie);
    }
}
