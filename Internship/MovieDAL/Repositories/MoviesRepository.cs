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

        private string stringConnection = "Server=tcp:andradadbserver.database.windows.net,1433;" +
                "Initial Catalog=MovieDb;Persist Security Info=False;User ID=admin123;Password=andrada123!" +
                ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private CustomApplicationSettings CustomApplicationSettings { get; }

        public MoviesRepository(IOptions<CustomApplicationSettings> customApplicationSettings)
        {
            CustomApplicationSettings = customApplicationSettings.Value;
        }




        public List<MovieModel> GetAll()
        {
            List<MovieModel> movieList = new List<MovieModel>();

            using (var connection = new SqlConnection(stringConnection))
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
                foreach (MovieModel m in movieList)
                {
                    
                    string queryStringForGenreList = string.Format("Select genre.GenreId, genre.GenreName from movie " + 
                        " Join movie_genre_junction on (movie.MovieId = movie_genre_junction.movieId)"+ "" +
                        "Join genre on(genre.genreId = movie_genre_junction.genreId)"+ 
                        "Where movie.MovieId = @id");
                    using (SqlCommand commandGenre = new SqlCommand(queryStringForGenreList, connection))
                    {
                        commandGenre.Parameters.AddWithValue("@id", m.MovieId);
                        using (SqlDataReader readerForGenreList = commandGenre.ExecuteReader())
                        {
                            while (readerForGenreList.Read())
                            {
                                GenreModel genre = new GenreModel();
                                genre.GenreId = Convert.ToInt32(readerForGenreList["GenreId"]);
                                genre.GenreName = readerForGenreList["GenreName"].ToString();
                                m.GenreList.Add(genre);
                            }
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

            using (var connection = new SqlConnection(stringConnection))
            {
                connection.Open();
                int idInserted = 0;
                string queryString = "INSERT INTO movie(Name, PremiereDate, Rating,DvdRelease)" +
                    "VALUES(@Name, @PremiereDate, @Rating, @DvdRelease) SELECT SCOPE_IDENTITY(); ";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Rating", model.Rating);
                    command.Parameters.AddWithValue("@PremiereDate", model.ReleaseDate);
                    command.Parameters.AddWithValue("@DvdRelease", model.ReleasedOnDvd);
                    idInserted = Convert.ToInt32(command.ExecuteScalar());

                }

                string queryForAddingGenre = "Insert into movie_genre_junction (movieId ,genreId) Values (@movieId, @genreId)";
                foreach (GenreModel g in model.GenreList)
                {
                    using (SqlCommand command = new SqlCommand(queryForAddingGenre, connection))
                    {
                        command.Parameters.AddWithValue("@movieId", idInserted);
                        command.Parameters.AddWithValue("@genreId", g.GenreId);
                        command.ExecuteScalar();
                    }
                }

                return idInserted;
            }
        }
        public void Edit(MovieModel movieModel)
        {
            if(movieModel == null)
            {
                return;
            }
            using (var connection = new SqlConnection(stringConnection))
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
                EditGenreList(movieModel);
               
            }

        }

        public void EditGenreList (MovieModel movie)
        {
            List<GenreModel> genreList = new List<GenreModel>();
            using (var connection = new SqlConnection(stringConnection))
            {
                connection.Open();
                string queryStringForGenreList = "Select genre.GenreId, genre.GenreName  from movie Join movie_genre_junction on (movie.MovieId = movie_genre_junction.movieId) Join genre on(genre.genreId = movie_genre_junction.genreId) Where movie.MovieId = @id";
                using (SqlCommand commandGenre = new SqlCommand(queryStringForGenreList.ToString(), connection))
                {
                    commandGenre.Parameters.AddWithValue("@id", movie.MovieId);
                    using (SqlDataReader readerForGenreList = commandGenre.ExecuteReader())
                    {
                        while (readerForGenreList.Read())
                        {
                            GenreModel genre = new GenreModel();
                            genre.GenreId = Convert.ToInt32(readerForGenreList["GenreId"]);
                            genre.GenreName = readerForGenreList["GenreName"].ToString();
                            genreList.Add(genre);
                        }
                    }
                }


                if(movie.GenreList.Count > genreList.Count)
                {
                    var DiffList = movie.GenreList.Except(genreList);
                    string queryForAddingGenre = "Insert into movie_genre_junction (movieId ,genreId) Values (@movieId, @genreId)";
                    foreach (GenreModel g in DiffList)
                    {
                        
                        
                            using (SqlCommand command = new SqlCommand(queryForAddingGenre, connection))
                            {
                                command.Parameters.AddWithValue("@movieId", movie.MovieId);
                                command.Parameters.AddWithValue("@genreId", g.GenreId);
                                command.ExecuteScalar();
                            }
                        
                    }

                }

                if(movie.GenreList.Count < genreList.Count)
                {
                    var DiffList = genreList.Except(movie.GenreList);
                    foreach (GenreModel g in DiffList)
                    {
                        string deleteJunction = "Delete from movie_genre_junction where movieId = @movieId AND  genreId =@genreId" ;
                        using (SqlCommand command = new SqlCommand(deleteJunction, connection))
                        {
                            command.Parameters.AddWithValue("@movieId",movie.MovieId);
                            command.Parameters.AddWithValue("@genreId", g.GenreId);

                            command.ExecuteNonQuery();

                        }
                    }

                }

                
                
            }
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                return false;
            }
            using (var connection = new SqlConnection(stringConnection))
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
