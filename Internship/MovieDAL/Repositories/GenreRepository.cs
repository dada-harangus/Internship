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

        private string stringConnection = "Server=tcp:andradadbserver.database.windows.net,1433;" +
                "Initial Catalog=MovieDb;Persist Security Info=False;User ID=admin123;Password=andrada123!" +
                ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public GenreRepository()
        {

        }
        public List<GenreModel> GetAll()
        {
            List< GenreModel> GenreList = new List<GenreModel>();

            var connection = new SqlConnection(stringConnection);
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
            reader.Close();
            connection.Close();
            return GenreList;
        }
    }
}
