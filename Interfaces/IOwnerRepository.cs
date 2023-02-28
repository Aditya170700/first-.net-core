using System;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface IOwnerRepository
	{
		ICollection<Owner> GetOwners();
		Owner GetOwner(int Id);
		ICollection<Owner> GetOwnerByPokemon(int pokemonId);
		ICollection<Pokemon> GetPokemonByOwner(int Id);
		bool OwnerExists(int Id);
		bool CreateOwner(Owner owner);
		bool Save();
		bool UpdateOwner(OwnerUpdateDto ownerUpdateDto, Owner owner);
	}
}

