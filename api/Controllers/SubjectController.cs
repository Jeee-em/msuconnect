using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Data.Repositories.Interfaces;
using api.Helpers;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ISubjectRepository _subjectRepo;
        public SubjectController(ApplicationDBContext context, ISubjectRepository subjectRepo)
        {
            _context = context;
            _subjectRepo = subjectRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            };

            var subjects = await _subjectRepo.GetAllSubjectsAsync(query);

            var subjectDto = subjects.Select(x => x.ToSubjectDto()).ToList();

            return Ok(subjectDto);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSubjectById([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var subject = await _subjectRepo.GetSubjectByIdAsync(id);

            if (subject == null) {
                return NotFound();
            }

            return Ok(subject.ToSubjectDto());
        }
        
    }
}