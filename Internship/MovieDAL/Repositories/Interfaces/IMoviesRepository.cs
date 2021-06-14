using MovieLibrary.Dto;
using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieDAL.Repositories
{
    public interface IMoviesRepository
    {
        public List<MovieModel> GetAll();
        public int Save(MovieDto model);

        public void Update(MovieDto movieModel);

        public bool Delete(int id);
    }
}
