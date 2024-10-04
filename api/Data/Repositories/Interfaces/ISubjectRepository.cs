using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models;

namespace api.Data.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllSubjectsAsync(QueryObject query);
        Task<Subject?> GetSubjectByIdAsync(int id); 
    }
}