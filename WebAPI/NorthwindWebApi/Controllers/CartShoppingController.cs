using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.DataTransferObject;
using AutoMapper;
using System.Collections.Generic;
using Northwind.Entities.Models;
using Northwind.Contracts;
using System.Linq;
using System;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartShoppingController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IServiceManager _service;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        Order order = new Order();
        OrderDetail orderDetail = new OrderDetail();
        Product product = new Product();

        public CartShoppingController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IServiceManager service)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var product = _service.GetAllProduct(trackChanges: false);

                var categoryDto = _mapper.Map<IEnumerable<ProductDto>>(product.Item2);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetAllProducts)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }//end class

        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] CartDto cartDto)
        {

            try
            {
                if (cartDto == null)
                {
                    _logger.LogError("OrdersDetail Object is null");
                    BadRequest("OrdersDetail Object is null");
                }
                var cartEntity = _service.AddToCart(cartDto.ProductId, cartDto.Qty, cartDto.CustomerId);

                return Ok(_mapper.Map<OrdersDetailDto>(cartEntity.Item2));
             }
             catch (Exception ex)
             {
                    _logger.LogError($"{nameof(AddToCart)} message : {ex}");
                     return StatusCode(500, $"Error {ex}");
             }

            }
    }
}
