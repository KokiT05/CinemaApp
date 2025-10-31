using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    [Comment("Movie in the system")]
    public class Movie
    {
        [Comment("Movie Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Movie Title")]
        public string Title { get; set; } = null!;

        [Comment("Movie Genre")]
        public string Genre { get; set; } = null!;

        [Comment("Movie Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Comment("Movie Director")]
        public string Director { get; set; } = null!;

        [Comment("Movie Duration")]
        public int Duration { get; set; }

        [Comment("Movie Description")]
        public string Description { get; set; } = null!;

        [Comment("Movie Image Url")]
        public string? ImageUrl { get; set; }

        [Comment("Movie Is Deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
