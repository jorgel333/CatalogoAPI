using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategorysController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public CategorysController(IUnityOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<CategoryDTO>> GertCategoryProduct()
        {
            var product = _uof.CategoryRepository.GetCategoryProduct().ToList();
            var productDto = _mapper.Map<List<CategoryDTO>>(product);

            return productDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> Get()
        {
            var category = _uof.CategoryRepository.Get().ToList();
            if (category is null)
            {
                return NotFound("Categorys not found...");
            }
            var categoryDto = _mapper.Map<List<CategoryDTO>>(category);
            return categoryDto;
        }

        [HttpGet("{id:int}", Name = "ObterCategory")]
        public ActionResult<CategoryDTO> GetCategory(int id)
        {
            var category = _uof.CategoryRepository.GetById(category => category.Id == id);
            if (category is null)
            {
                return NotFound("Category not found...");
            }
            
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return categoryDto;
        }

        [HttpPost]
        public ActionResult Post(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _uof.CategoryRepository.Add(category);
            _uof.Commit();

            var categoryDto2 = _mapper.Map<CategoryDTO>(category);
            return new CreatedAtRouteResult("ObterCategory",
                new { id = category.Id }, categoryDto2);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(categoryDto);

            _uof.CategoryRepository.Update(category);
            _uof.Commit();

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _uof.CategoryRepository.GetById(category => category.Id == id);

            if (category is null)
            {
                return NotFound("Category not found...");
            }

            _uof.CategoryRepository.Delete(category);
            _uof.Commit();

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDto);
        }
    }
}
