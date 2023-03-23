using APICatalogo.Context;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Repository;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnityOfWork _uof;

        public ProductsController(IUnityOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("lowestprice")]
        public ActionResult<IEnumerable<Product>> GetOrderProductPrice()
        {
            return _uof.ProductRepository.GetOrderProductsByPrice().ToList();
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var product = _uof.ProductRepository.Get().ToList();
            if(product is null)
            {
                return NotFound("Products not found...");
            }
            return product;
        }

        [HttpGet("{id:int}", Name = "ObterProduct")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _uof.ProductRepository.GetById(product => product.Id == id);
            if(product is null)
            {
                return NotFound("Product not found...");
            }
            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            
            _uof.ProductRepository.Add(product);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduct", 
                new {id = product.Id}, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            _uof.ProductRepository.Update(product);
            _uof.Commit();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _uof.ProductRepository.GetById(product => product.Id == id);

            if (product is null)
            {
                return NotFound("Category not found...");
            }

            _uof.ProductRepository.Delete(product);
            _uof.Commit();
            
            return Ok(product);
        }
    }
}
