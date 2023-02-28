using System;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class PokemonRepository : IPokemonRepository
	{
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
		{
			_context = context;
		}

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Pokemon GetById (int Id)
        {
            return _context.Pokemons
                .Where(p => p.Id == Id)
                .FirstOrDefault();
        }

        public decimal GetPokemonRating (int Id)
        {
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == Id);
            var reviewCount = reviews.Count();

            if (reviewCount <= 0)
                return 0;

            return (decimal)reviews.Sum(r => r.Rating) / reviewCount;
        }

        public bool PokemonExists(int Id)
        {
            return _context.Pokemons
                .Any(p => p.Id == Id);
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon,
            };
            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemonOwner);
            _context.Add(pokemonCategory);
            _context.Add(pokemon);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdatePokemon(PokemonUpdateDto pokemonUpdateDto, Pokemon pokemon)
        {
            pokemon.Name = pokemonUpdateDto.Name;
            pokemon.BirthDate = pokemonUpdateDto.BirthDate;

            return Save();
        }
    }
}
