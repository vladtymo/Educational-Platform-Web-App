using SS_EDUP.Core.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Interfaces
{
    public interface ILearningService
    {
        Task<List<LearningDto>> GetAll();
        Task<LearningDto?> Get(int id);
        Task Add( string studentId, params int[] ids);
        Task Update(LearningDto learning);
        Task Delete(int id);
        Task<List<LearningDto>> GetByStudentId(string id);
        Task<LearningDto?> GetByCourse(int id);
    }
}
