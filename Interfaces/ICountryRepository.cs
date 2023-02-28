using System;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
	public interface ICountryRepository
	{
		ICollection<Country> GetCountries();
		Country GetCountry(int Id);
		ICollection<Owner> GetOwnerByCountry(int Id);
		bool CountryExists(int Id);
		bool CreateCountry(Country country);
		bool Save();
		bool UpdateCountry(CountryUpdateDto countryUpdateDto, Country country);
	}
}

