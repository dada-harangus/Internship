using Microsoft.Extensions.Options;
using MovieDAL.Repositories.Interfaces;
using MovieLibrary;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDAL.Repositories
{
    public class MovieGenreRepository : IMovieGenreRepository
    {
        private ConectionString connectionString { get; }
        public MovieGenreRepository(IOptions<ConectionString> connectionString)
        {
            this.connectionString = connectionString.Value;
        }

        public void AddMovieGenre(int idMovie, int idGenre)
        {
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string queryForAddingGenre = "Insert into movie_genre_junction (movieId ,genreId) Values (@movieId, @genreId)";

                using (SqlCommand command = new SqlCommand(queryForAddingGenre, connection))
                {
                    command.Parameters.AddWithValue("@movieId", idMovie);
                    command.Parameters.AddWithValue("@genreId", idGenre);
                    command.ExecuteScalar();
                }

            }
        }


        public void DeleteMovieGenreConnection(int idMovie, int idGenre)
        {

            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string deleteJunction = "Delete from movie_genre_junction where movieId = @movieId AND  genreId =@genreId";
                using (SqlCommand command = new SqlCommand(deleteJunction, connection))
                {
                    command.Parameters.AddWithValue("@movieId", idMovie);
                    command.Parameters.AddWithValue("@genreId", idGenre);
                    command.ExecuteNonQuery();
                }
            }

        }

        public List<int> GetAllGenrePerMovieIds(int idMovie)
        {
            List<int> idList = new List<int>();
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string getIdstring = "Select genreId From movie_genre_Junction Where movieId = @movieId";
                using (SqlCommand command = new SqlCommand(getIdstring, connection))
                {
                    command.Parameters.AddWithValue("@movieId", idMovie);


                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        idList.Add(Convert.ToInt32(reader["genreId"]));
                    }

                }


            }
            return idList;

        }
    }
}

