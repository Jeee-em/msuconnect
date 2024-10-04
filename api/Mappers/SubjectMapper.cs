using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Subject;
using api.Models;

namespace api.Mappers
{
    public static class SubjectMapper
    {
        public static SubjectDto ToSubjectDto(this Subject subjectModel)
        {
            return new SubjectDto
            {
                SubjectId = subjectModel.SubjectId,
                SubjectName = subjectModel.SubjectName,
                Description = subjectModel.Description,
                MentorId = subjectModel.MentorId,
                Mentor = subjectModel.Mentor
            };
        }
    }
}