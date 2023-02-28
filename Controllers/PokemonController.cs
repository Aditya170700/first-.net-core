using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
	[Route("api/pokemons")]
	[ApiController]
	public class PokemonController : Controller
	{
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IMapper mapper, IPokemonRepository pokemonRepository)
		{
            _mapper = mapper;
			_pokemonRepository = pokemonRepository;
        }

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
		public IActionResult GetPokemons()
		{
			var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemons);
		}

		[HttpGet("{Id}")]
		[ProducesResponseType(200, Type = typeof(PokemonDto))]
		[ProducesResponseType(400)]
		public IActionResult GetPokemon(int Id)
		{
			if (!_pokemonRepository.PokemonExists(Id))
				return NotFound();

			var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetById(Id));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemon);
        }

        [HttpGet("{Id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
		public IActionResult GetPokemonRating(int Id)
        {
            if (!_pokemonRepository.PokemonExists(Id))
                return NotFound();

			var rating = _pokemonRepository.GetPokemonRating(Id);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] PokemonCreateDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemon = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("error", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(pokemonCreate.ownerId, pokemonCreate.categoryId, pokemonMap))
            {
                ModelState.AddModelError("error", "Something went wrong while saving data");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdatePokemon(int Id, [FromBody] PokemonUpdateDto pokemonUpdate)
        {
            if (pokemonUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonUpdate.Name.Trim().ToUpper())
                .Where(p => p.BirthDate == pokemonUpdate.BirthDate)
                .Where(p => p.Id != Id)
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("error", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            var existingData = _pokemonRepository.GetById(Id);

            if (existingData == null)
            {
                ModelState.AddModelError("error", "Pokemon not found");
                return StatusCode(404, ModelState);
            }

            if (!_pokemonRepository.UpdatePokemon(pokemonUpdate, existingData))
            {
                ModelState.AddModelError("error", "Something went wrong while saving data");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
    }
}

