using System;
namespace PokemonReviewApp.Dto
{
	public class OwnerCreateDto
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public int CountryId { get; set; }
    }
}

