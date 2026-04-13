using AspNetCoreArchTemplate.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AspNetCoreArchTemplate.Data.Common.EntityConstants.Movie;

namespace AspNetCoreArchTemplate.Data.Configuration
{
	public class MovieConfiguration : IEntityTypeConfiguration<Movie>
	{
		public void Configure(EntityTypeBuilder<Movie> builder)
		{
			builder.HasKey(m => m.Id);

			builder.Property(m => m.Title)
					.IsRequired()
					.HasMaxLength(TitleMaxLength);

			builder.Property(m => m.Genre)
					.IsRequired()
					.HasMaxLength(GenreMaxLength);

			builder.Property(m => m.ReleaseDate)
				.IsRequired()
				.HasDefaultValue(DateTime.Now);

			builder.Property(m => m.Director)
					.IsRequired()
					.HasMaxLength(DirectorMaxLength);

			builder.Property(m => m.Duration)
					.IsRequired()
					.HasMaxLength(DurationMax);

			builder.Property(m => m.Description)
					.IsRequired()
					.HasMaxLength(DescriptionMaxLength);

			builder.Property(m => m.ImageUrl)
					.HasMaxLength(ImageUrlMaxValue);

			builder.Property(m => m.IsDeleted)
					.IsRequired()
					.HasDefaultValue(false);
		}
	}
}
