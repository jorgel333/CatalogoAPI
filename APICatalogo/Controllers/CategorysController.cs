using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GertCategoryProduct()
        {
            var product = await _uof.CategoryRepository.GetCategoryProduct();
            var productDto = _mapper.Map<List<CategoryDTO>>(product);

            return productDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategorysParameters categorysParameters)
        {
            var category = await _uof.CategoryRepository.GetCategorys(categorysParameters);
            if (category is null)
            {
                return NotFound("Categorys not found...");
            }

            var metadata = new
            {
                category.TotalCount,
                category.PageSize,
                category.CurrentPage,
                category.TotalPages,
                category.HasPrevious,
                category.HasNext
            };

            Response.Headers.Add("X-pagination", JsonConvert.SerializeObject(metadata));

            var categoryDto = _mapper.Map<List<CategoryDTO>>(category);
            return categoryDto;
        }

        [HttpGet("{id:int}", Name = "ObterCategory")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _uof.CategoryRepository.GetById(category => category.Id == id);
            if (category is null)
            {
                return NotFound("Category not found...");
            }
            
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return categoryDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _uof.CategoryRepository.Add(category);
            await _uof.Commit();

            var categoryDto2 = _mapper.Map<CategoryDTO>(category);
            return new CreatedAtRouteResult("ObterCategory",
                new { id = category.Id }, categoryDto2);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(categoryDto);

            _uof.CategoryRepository.Update(category);
            await _uof.Commit();

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _uof.CategoryRepository.GetById(category => category.Id == id);

            if (category is null)
            {
                return NotFound("Category not found...");
            }

            _uof.CategoryRepository.Delete(category);
            await _uof.Commit();

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDto);
        }
    }
}
