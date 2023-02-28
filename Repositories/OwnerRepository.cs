using System;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class OwnerRepository : IOwnerRepository
	{
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
		{
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateOwner(OwnerUpdateDto ownerUpdateDto, Owner owner)
        {
            owner.FirstName = ownerUpdateDto.FirstName;
            owner.LastName = ownerUpdateDto.LastName;
            owner.Gym = ownerUpdateDto.Gym;
            owner.CountryId = ownerUpdateDto.CountryId;

            return Save();
        }

        Owner IOwnerRepository.GetOwner(int Id)
        {
            return _context.Owners
                .Where(o => o.Id == Id)
                .FirstOrDefault();
        }

        ICollection<Owner> IOwnerRepository.GetOwnerByPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                .Where(po => po.PokemonId == pokemonId)
                .Select(po => po.Owner)
                .OrderBy(o => o.Id)
                .ToList();
        }

        ICollection<Owner> IOwnerRepository.GetOwners()
        {
            return _context.Owners
                .OrderBy(o => o.Id)
                .ToList();
        }

        ICollection<Pokemon> IOwnerRepository.GetPokemonByOwner(int Id)
        {
            return _context.PokemonOwners
                .Where(po => po.OwnerId == Id)
                .Select(po => po.Pokemon)
                .OrderBy(p => p.Id)
                .ToList();
        }

        bool IOwnerRepository.OwnerExists(int Id)
        {
            return _context.Owners.Any(o => o.Id == Id);
        }
    }
}

