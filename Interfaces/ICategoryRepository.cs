using System;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface ICategoryRepository
	{
		ICollection<Category> GetCategories();
		Category GetCategory(int Id);
		ICollection<Pokemon> GetPokemonByCategory(int Id);
		bool CategoryExists(int Id);
		bool CategoryCreate(Category category);
		bool Save();
		bool UpdateCategory(CategoryUpdateDto data, Category category);
	}
}

