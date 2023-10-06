using AutoMapper;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepo;
       

        public CategoriesService(IRepository<Category> categoryRepo, IMapper mapper)
        {
           this._categoryRepo = categoryRepo;
            this._mapper = mapper;
        }
        public async Task Create(CategoryDto categoryDto)
        {
            // create category in db
           await _categoryRepo.Insert(_mapper.Map<Category>(categoryDto));
           await _categoryRepo.Save(); // submit changes in db
        }

        public async Task Delete(int id)
        {
            var categoryDto = await Get(id);

            if (categoryDto == null) return; // exception

            await _categoryRepo.Delete(id);
            await _categoryRepo.Save();
        }

        public async Task<CategoryDto?> Get(int id)
        {
            if (id < 0) return null; // exception handling

            var category = await _categoryRepo.GetByID(id);

            if (category == null) return null; // exception handling

            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<List<CategoryDto>> GetAll()
        {
            var result = await  _categoryRepo.GetAll();//.ToList();
            return _mapper.Map<List<CategoryDto>>(result);
        }

        public async Task Update(CategoryDto categoryDto)
        {
            await _categoryRepo.Update(_mapper.Map<Category>(categoryDto));
            await _categoryRepo.Save();
        }
    }
}
