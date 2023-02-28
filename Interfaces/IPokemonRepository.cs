using System;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface IPokemonRepository
	{
		ICollection<Pokemon> GetPokemons();
		Pokemon GetById(int Id);
		decimal GetPokemonRating(int Id);
		bool PokemonExists(int Id);
		bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
		bool Save();
		bool UpdatePokemon(PokemonUpdateDto pokemonUpdateDto, Pokemon pokemon);
	}
}

