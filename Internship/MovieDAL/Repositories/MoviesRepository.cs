using Microsoft.Extensions.Options;
using MovieLibrary;
using MovieLibrary.Dto;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace MovieDAL.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {

        private ConectionString connectionString { get; }


        public MoviesRepository(IOptions<ConectionString> connectionString)
        {
            this.connectionString = connectionString.Value;
        }




        public List<MovieModel> GetAll()
        {
            List<MovieModel> movieList = new List<MovieModel>();

            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string queryString = "SELECT * FROM movie;";


                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            MovieModel movie = new MovieModel();
                            movie.MovieId = Convert.ToInt32(reader["movieId"].ToString());
                            movie.Name = reader["Name"].ToString();
                            movie.Rating = int.Parse(reader["Rating"].ToString());
                            movie.ReleaseDate = DateTime.Parse(reader["PremiereDate"].ToString());
                            movie.ReleasedOnDvd = bool.Parse(reader["DvdRelease"].ToString());
                            movieList.Add(movie);
                        }
                    }

                }


            }
            return movieList;
        }
        public int Save(MovieDto model)
        {
            if (model == null)
            {
                return 0;
            }

            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                int idInserted = 0;
                string queryString = @"INSERT INTO movie(Name, PremiereDate, Rating,DvdRelease)
                     VALUES(@Name, @PremiereDate, @Rating, @DvdRelease) 
                     SELECT SCOPE_IDENTITY(); ";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Rating", model.Rating);
                    command.Parameters.AddWithValue("@PremiereDate", model.ReleaseDate);
                    command.Parameters.AddWithValue("@DvdRelease", model.ReleasedOnDvd);
                    idInserted = Convert.ToInt32(command.ExecuteScalar());

                }

                return idInserted;
            }
        }
        public void Update(MovieDto movieModel)
        {
            if (movieModel == null)
            {
                return;
            }
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string queryString = "UPDATE movie SET Name = @Name, Rating = @Rating, PremiereDate = @PremiereDate , DvdRelease = @DvdRelease WHERE  movieId = @id; ";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Name", movieModel.Name);
                    command.Parameters.AddWithValue("@Rating", movieModel.Rating);
                    command.Parameters.AddWithValue("@PremiereDate", movieModel.ReleaseDate);
                    command.Parameters.AddWithValue("@DvdRelease", movieModel.ReleasedOnDvd);
                    command.Parameters.AddWithValue("@id", movieModel.MovieId);
                    command.ExecuteNonQuery();
                }


            }

        }



        public bool Delete(int id)
        {
            if (id == 0)
            {
                return false;
            }
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string queryString = "DELETE FROM movie WHERE  movieId = @id; ";
                bool rowsAffected = false;
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = command.ExecuteNonQuery();
                    rowsAffected = count > 0;

                }

                return rowsAffected;
            }

        }
    }
}
