using System;

namespace PokemonReviewApp.Dto
{
	public class PokemonCreateDto
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int ownerId { get; set; }
        public int categoryId { get; set; }
    }
}

