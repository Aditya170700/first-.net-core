using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/owners")]
    [ApiController]
    public class OwnerController : Controller
	{
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IMapper mapper, IOwnerRepository ownerRepository, ICountryRepository countryRepository)
		{
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int Id)
        {
            if (!_ownerRepository.OwnerExists(Id))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("by-pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        public IActionResult GetOwnerByPokemon(int pokemonId)
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerByPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{Id}/pokemon")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        public IActionResult GetPokemonByOwner(int Id)
        {
            var pokemons= _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody] OwnerCreateDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners()
                .Where(o => o.FirstName.Trim().ToUpper() == ownerCreate.FirstName.Trim().ToUpper())
                .Where(o => o.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper())
                .Where(o => o.Gym.Trim().ToUpper() == ownerCreate.Gym.Trim().ToUpper())
                .Where(o => o.Country.Id == ownerCreate.CountryId)
                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("error", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(ownerCreate.CountryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
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
        public IActionResult UpdateOwner(int Id, [FromBody] OwnerUpdateDto ownerUpdate)
        {
            if (ownerUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _ownerRepository.GetOwners()
                .Where(o => o.FirstName.Trim().ToUpper() == ownerUpdate.FirstName.Trim().ToUpper())
                .Where(o => o.LastName.Trim().ToUpper() == ownerUpdate.LastName.Trim().ToUpper())
                .Where(o => o.Id != Id)
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("error", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            var existingData = _ownerRepository.GetOwner(Id);

            if (existingData == null)
            {
                ModelState.AddModelError("error", "Owner not found");
                return StatusCode(404, ModelState);
            }

            if (!_ownerRepository.UpdateOwner(ownerUpdate, existingData))
            {
                ModelState.AddModelError("error", "Something went wrong while saving data");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
    }
}

