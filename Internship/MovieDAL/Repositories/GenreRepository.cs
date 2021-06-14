using Microsoft.Extensions.Options;
using MovieLibrary;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDAL.Repositories.Interfaces
{
    public class GenreRepository : IGenreRepository
    {

        private ConectionString connectionString { get; }
        public GenreRepository(IOptions<ConectionString> connectionString)
        {
            this.connectionString = connectionString.Value;
        }
        public List<GenreModel> GetAll()
        {
            List<GenreModel> GenreList = new List<GenreModel>();

            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string queryString = "SELECT * FROM GENRE;";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                //var check = connection.State == ConnectionState.Open; 
                while (reader.Read())
                {

                    GenreModel genre = new GenreModel();
                    genre.GenreId = Int32.Parse(reader["genreId"].ToString());
                    genre.GenreName = reader["GenreName"].ToString();

                    GenreList.Add(genre);

                }
            }
            return GenreList;
        }


        public GenreModel GetGendre(int id)
        {

            string queryStringForGenreList = @"Select * from Genre 
                                              Where GenreId = @id ";
            GenreModel genre = new GenreModel();
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                using (SqlCommand commandGenre = new SqlCommand(queryStringForGenreList, connection))
                {
                    commandGenre.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader readerForGenreList = commandGenre.ExecuteReader())
                    {
                        while (readerForGenreList.Read())
                        {
                            
                            genre.GenreId = Convert.ToInt32(readerForGenreList["GenreId"]);
                            genre.GenreName = readerForGenreList["GenreName"].ToString();
                            
                        }
                    }
                }
            }
            return genre;

        }

        public int AddGenre(string foundGenre)
        {
            using (var connection = new SqlConnection(connectionString.Setting1))
            {
                connection.Open();
                string addGenre = @" Insert into genre (genreName)
                                     values (@GenreName)
                                     SELECT SCOPE_IDENTITY();   ";
                using (SqlCommand commandGenre = new SqlCommand(addGenre, connection))
                {
                    commandGenre.Parameters.AddWithValue("@GenreName", foundGenre);
                    int idInserted = Convert.ToInt32(commandGenre.ExecuteScalar());
                    return idInserted;
                }

            }

        }

       
    }
}
