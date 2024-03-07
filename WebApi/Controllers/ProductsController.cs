using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get(int? categoryId = null,
            int? minPrice = null,
            int? maxPrice = null)
        {
            var filterSearch = new FilterSearchModel()
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var products = await _productService.GetByFilterAsync(filterSearch);

            return Ok(products);
        }

        //GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null) return NotFound("Product not found");

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel value)
        {
            await _productService.AddAsync(value);

            return Ok(value);
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ProductModel value)
        {
            value.Id = Id;

            await _productService.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return Ok();
        }

        //GET: api/products/categories 
        [HttpGet("Categories")]
        public async Task<ActionResult<ProductCategoryModel>> GetCategories()
        {
            return Ok(await _productService.GetAllAsync());
        }

        // POST: api/products/categories
        [HttpPost("Categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
        {
            await _productService.AddCategoryAsync(value);

            return Ok(value);
        }

        // PUT: api/products/categories?1
        [HttpPut("Categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel value)
        {
            value.Id = Id;

            await _productService.UpdateCategoryAsync(value);

            return Ok();
        }

        // DELETE: api/products/categories/1
        [HttpDelete("Categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _productService.RemoveCategoryAsync(id);

            return Ok();
        }
    }
}
