using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    [Route("api/reviewers")]
    [ApiController]
    public class ReviewerController : Controller
	{
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IMapper mapper, IReviewerRepository reviewerRepository)
		{
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int Id)
        {
            if (!_reviewerRepository.ReviewerExists(Id))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet("{Id}/review")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewByReviewer(int Id)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewByReviewer(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] ReviewerCreateDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers()
                .Where(r => r.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.Trim().ToUpper())
                .Where(r => r.LastName.Trim().ToUpper() == reviewerCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("error", "Reveiwer already exists");
                return StatusCode(422, ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
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
        public IActionResult UpdateReviewer(int Id, [FromBody] ReviewerUpdateDto reviewerUpdate)
        {
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _reviewerRepository.GetReviewers()
                .Where(r => r.FirstName.Trim().ToUpper() == reviewerUpdate.FirstName.Trim().ToUpper())
                .Where(r => r.LastName.Trim().ToUpper() == reviewerUpdate.LastName.Trim().ToUpper())
                .Where(r => r.Id != Id)
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("error", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            var existingData = _reviewerRepository.GetReviewer(Id);

            if (existingData == null)
            {
                ModelState.AddModelError("error", "Reviewer not found");
                return StatusCode(404, ModelState);
            }

            if (!_reviewerRepository.UpdateReviewer(reviewerUpdate, existingData))
            {
                ModelState.AddModelError("error", "Something went wrong while saving data");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
    }
}

