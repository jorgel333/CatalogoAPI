using APICatalogo.Context;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Repository;
using APICatalogo.DTOs;
using AutoMapper;
using APICatalogo.Pagination;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace APICatalogo.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetOrderProductPrice()
        {
            var product = await _uof.ProductRepository.GetOrderProductsByPrice();
            var productDto =  _mapper.Map<List<ProductDTO>>(product);
            return productDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> 
            Get([FromQuery] ProductsParameters productsParameters)
        {
            var product = await _uof.ProductRepository.GetProducts(productsParameters);

            var metadata = new
            {
                product.TotalCount,
                product.PageSize,
                product.CurrentPage,
                product.TotalPages,
                product.HasPrevious,
                product.HasNext
            };

            Response.Headers.Add("X-pagination", JsonConvert.SerializeObject(metadata));
            var productDto = _mapper.Map<List<ProductDTO>>(product);

            if (product is null)
            {
                return NotFound("Products not found...");
            }
            return productDto;
        }

        [HttpGet("{id:int}", Name = "ObterProduct")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _uof.ProductRepository.GetById(product => product.Id == id);
            if (product is null)
            {
                return NotFound("Product not found...");
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Add(product);
            await _uof.Commit();

            var productDto2 = _mapper.Map<ProductDTO>(product);
            return new CreatedAtRouteResult("ObterProduct",
                new { id = product.Id }, productDto2);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductDTO productDto)
        {

            if (id != productDto.Id) 
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Update(product);
            await _uof.Commit();

            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _uof.ProductRepository.GetById(product => product.Id == id);

            if (product is null)
            {
                return NotFound("Category not found...");
            }

            _uof.ProductRepository.Delete(product);
            await _uof.Commit();

            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }
    }
}
