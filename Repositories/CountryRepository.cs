using System;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
	public class CountryRepository : ICountryRepository
	{
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
		{
            _context = context;
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateCountry(CountryUpdateDto countryUpdateDto, Country country)
        {
            country.Name = countryUpdateDto.Name;

            return Save();
        }

        bool ICountryRepository.CountryExists(int Id)
        {
            return _context.Countries
                .Any(c => c.Id == Id);
        }

        ICollection<Country> ICountryRepository.GetCountries()
        {
            return _context.Countries
                .OrderBy(c => c.Id)
                .ToList();
        }

        Country ICountryRepository.GetCountry(int Id)
        {
            return _context.Countries
                .Where(c => c.Id == Id)
                .FirstOrDefault();
        }

        ICollection<Owner> ICountryRepository.GetOwnerByCountry(int Id)
        {
            return _context.Owners
                .Where(o => o.Country.Id == Id)
                .OrderBy(o => o.Id)
                .ToList();
        }
    }
}

