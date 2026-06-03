using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
	public class UserMovieConfiguration : IEntityTypeConfiguration<UserMovie>
	{
		public void Configure(EntityTypeBuilder<UserMovie> entity)
		{
			entity.HasKey(um => new { um.UserId, um.MovieId });

			entity.HasOne(um => um.IdentityUser)
				.WithMany()
				.HasForeignKey(um => um.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(um => um.Movie)
				.WithMany()
				.HasForeignKey(um => um.MovieId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
