using System;
using System.Diagnostics.Metrics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : Controller
	{
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountryController(IMapper mapper, ICountryRepository countryRepository)
		{
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries ()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry (int Id)
        {
            if (!_countryRepository.CountryExists(Id))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("{Id}/owner")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        public IActionResult GetOwnerByCountry(int Id)
        {
            var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnerByCountry(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] CountryCreateDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("error", "Country already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
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
        public IActionResult UpdateCountry(int Id, [FromBody] CountryUpdateDto countryUpdate)
        {
            if (countryUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryUpdate.Name.Trim().ToUpper())
                .Where(c => c.Id != Id)
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("error", "Country already exists");
                return StatusCode(422, ModelState);
            }

            var existingData = _countryRepository.GetCountry(Id);

            if (existingData == null)
            {
                ModelState.AddModelError("error", "Country not found");
                return StatusCode(404, ModelState);
            }

            if (!_countryRepository.UpdateCountry(countryUpdate, existingData))
            {
                ModelState.AddModelError("error", "Something went wrong while saving data");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
    }
}

