using APICatalogo.Context;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Repository;
using APICatalogo.DTOs;
using AutoMapper;
using APICatalogo.Pagination;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public ProductsController(IUnityOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("lowestprice")]
        public ActionResult<IEnumerable<ProductDTO>> GetOrderProductPrice()
        {
            var product = _uof.ProductRepository.GetOrderProductsByPrice().ToList();
            var productDto = _mapper.Map<List<ProductDTO>>(product);
            return productDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var product = _uof.ProductRepository.GetProducts(productsParameters).ToList();
            var productDto = _mapper.Map<List<ProductDTO>>(product);

            if (product is null)
            {
                return NotFound("Products not found...");
            }
            return productDto;
        }

        [HttpGet("{id:int}", Name = "ObterProduct")]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            var product = _uof.ProductRepository.GetById(product => product.Id == id);
            if (product is null)
            {
                return NotFound("Product not found...");
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }

        [HttpPost]
        public ActionResult Post(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Add(product);
            _uof.Commit();

            var productDto2 = _mapper.Map<ProductDTO>(product);
            return new CreatedAtRouteResult("ObterProduct",
                new { id = product.Id }, productDto2);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProductDTO productDto)
        {

            if (id != productDto.Id) 
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Update(product);
            _uof.Commit();

            return Ok(productDto);
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

            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }
    }
}
