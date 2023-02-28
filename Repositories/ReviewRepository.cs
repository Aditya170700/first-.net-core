using System;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class ReviewRepository : IReviewRepository
	{
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
		{
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        Review IReviewRepository.GetReview(int Id)
        {
            return _context.Reviews
                .Where(r => r.Id == Id)
                .FirstOrDefault();
        }

        ICollection<Review> IReviewRepository.GetReviewByPokemon(int pokemonId)
        {
            return _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .OrderBy(r => r.Id)
                .ToList();
        }

        ICollection<Review> IReviewRepository.GetReviews()
        {
            return _context.Reviews
                .OrderBy(r => r.Id)
                .ToList();
        }

        bool IReviewRepository.ReviewExists(int Id)
        {
            return _context.Reviews
                .Any(r => r.Id == Id);
        }
    }
}

