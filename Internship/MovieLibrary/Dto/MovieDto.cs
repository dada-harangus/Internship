using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Dto
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        [Required(ErrorMessage = "An movie name  is required")]
        public string Name { get; set; }

        [DateAttribute]
        public DateTime ReleaseDate { get; set; }
        [Range(1, 5,
            ErrorMessage = "Rating must be between 0 and 5")]
        public int Rating { get; set; }
        public bool ReleasedOnDvd { get; set; }

        public List<string> GenreList { get; set; } = new List<string>();


    }
}
