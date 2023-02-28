using System;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface IReviewerRepository
	{
		ICollection<Reviewer> GetReviewers();
		Reviewer GetReviewer(int Id);
		ICollection<Review> GetReviewByReviewer(int Id);
		bool ReviewerExists(int Id);
		bool CreateReviewer(Reviewer reviewer);
		bool Save();
	}
}

