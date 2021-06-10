using System;
using System.Collections.Generic;

namespace MovieLibrary.Models
{
    public class MovieModel
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int Rating { get; set; }
        public bool ReleasedOnDvd { get; set; }

        public List<GenreModel> GenreList { get; set; } = new List<GenreModel>();
    }
}
