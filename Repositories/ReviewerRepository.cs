using System;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class ReviewerRepository : IReviewerRepository
	{
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
		{
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        ICollection<Review> IReviewerRepository.GetReviewByReviewer(int Id)
        {
            return _context.Reviews
                .Where(r => r.Reviewer.Id == Id)
                .OrderBy(r => r.Id)
                .ToList();
        }

        Reviewer IReviewerRepository.GetReviewer(int Id)
        {
            return _context.Reviewers
                .Where(r => r.Id == Id)
                .Include(r => r.Reviews)
                .FirstOrDefault();
        }

        ICollection<Reviewer> IReviewerRepository.GetReviewers()
        {
            return _context.Reviewers
                .OrderBy(r => r.Id)
                .ToList();
        }

        bool IReviewerRepository.ReviewerExists(int Id)
        {
            return _context.Reviewers
                .Any(r => r.Id == Id);
        }
    }
}

