using System;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface IReviewRepository
	{
		ICollection<Review> GetReviews();
		Review GetReview(int Id);
		ICollection<Review> GetReviewByPokemon(int pokemonId);
		bool ReviewExists(int Id);
		bool CreateReview(Review review);
		bool Save();
	}
}

