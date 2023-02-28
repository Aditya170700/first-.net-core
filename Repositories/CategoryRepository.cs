using System;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
		{
            _context = context;
        }

        public bool CategoryCreate(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateCategory(CategoryUpdateDto data, Category category)
        {
            category.Name = data.Name;

            return Save();
        }

        bool ICategoryRepository.CategoryExists(int Id)
        {
            return _context.Categories
                .Any(c => c.Id == Id);
        }

        ICollection<Category> ICategoryRepository.GetCategories()
        {
            return _context.Categories
                .OrderBy(c => c.Id)
                .ToList();
        }

        Category ICategoryRepository.GetCategory(int Id)
        {
            return _context.Categories
                .Where(c => c.Id == Id)
                .FirstOrDefault();
        }

        ICollection<Pokemon> ICategoryRepository.GetPokemonByCategory(int Id)
        {
            return _context.PokemonCategories
                .Where(pc => pc.CategoryId == Id)
                .Select(pc => pc.Pokemon)
                .OrderBy(pc => pc.Id)
                .ToList();
        }
    }
}

