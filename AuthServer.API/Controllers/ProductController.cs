using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using AuthServer.CoreLayer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productService.AddAsync(productDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            return ActionResultInstance(await _productService.Remove(Id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto, int id)
        {
            return ActionResultInstance(await _productService.UpdateAsync(productDto, id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return ActionResultInstance(await _productService.GetByIdAsync(id));
        }
    }
}
