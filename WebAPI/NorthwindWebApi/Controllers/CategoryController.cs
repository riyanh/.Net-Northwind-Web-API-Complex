using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using System;
using System.Linq;
using Northwind.Entities.DataTransferObject;
using AutoMapper;
using System.Collections.Generic;

namespace NorthwindWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _repository.Category.GetAllCategory(trackChanges: false);

                //replace by categoryDto
                /*var categoryDto = categories.Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    categoryName = c.CategoryName,
                    description = c.Description
                }).ToList();*/

                var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCategories)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
