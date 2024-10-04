using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data.Repositories.Interfaces;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository 
    {
        private readonly ApplicationDBContext _context;
        public SubjectRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Subject>> GetAllSubjectsAsync(QueryObject query)
        {
            var subjects = _context.Subjects.ToListAsync();

            return await subjects;
        }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);
        }
    }
}