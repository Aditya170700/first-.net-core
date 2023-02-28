using System;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
	public class OwnerUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public int CountryId { get; set; }
    }
}

