using SS_EDUP.Core.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoryDto>> GetAll();
       
        Task<CategoryDto?> Get(int id);
        Task Create(CategoryDto categoryDto);
        Task Update(CategoryDto categoryDto);
        Task Delete(int id);
    }
}
