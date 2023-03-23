using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
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

        public CategorysController(IUnityOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GertCategoryProduct()
        {
            return _uof.CategoryRepository.GetCategoryProduct().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var category = _uof.CategoryRepository.Get().ToList();
            if (category is null)
            {
                return NotFound("Categorys not found...");
            }
            return category;
        }

        [HttpGet("{id:int}", Name = "ObterCategory")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _uof.CategoryRepository.GetById(category => category.Id == id);
            if (category is null)
            {
                return NotFound("Category not found...");
            }
            return category;
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            _uof.CategoryRepository.Add(category);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategory",
                new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _uof.CategoryRepository.Update(category);
            _uof.Commit();

            return Ok(category);
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

            return Ok(category);
        }
    }
}
