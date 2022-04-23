using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using System;
using System.Linq;
using Northwind.Entities.DataTransferObject;
using AutoMapper;
using System.Collections.Generic;
using Northwind.Entities.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Northwind.Entities.RequestFeatures;

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
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _repository.Category.GetAllCategoryAsync(trackChanges: false);

                var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCategories)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }//EndMethodGetCategories

        [HttpGet("{id}",Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _repository.Category.GetCategoryAsync(id, trackChanges: false);
            if(category == null)
            {
                _logger.LogInfo($"Category with Id : {id} doesn't exist");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
        }//endMethod GetCategory parameter id

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogError("Category object is null");
                return BadRequest("Category object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid modelstate categoryDto");
                return UnprocessableEntity(ModelState);
            }

            var categoryEntity = _mapper.Map<Category>(categoryDto);
            _repository.Category.CreateCategoryAsync(categoryEntity);
            await _repository.SaveAsync();

            var categoryResult = _mapper.Map<CategoryDto>(categoryEntity);
            return CreatedAtRoute("CategoryById", new { id = categoryResult.categoryId }, categoryResult);
        }//endMethod CreateCategory

        [HttpDelete]
       public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategoryAsync(id, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id {id} doesn't exist in database");
                return NotFound();
            }

            _repository.Category.DeleteCategoryAsync(category);
            await _repository.SaveAsync();
            return NoContent();
        }//endMethodDelete

        [HttpPut("{id}")]
       public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryDto)
       {
           if(categoryDto == null)
            {
                _logger.LogError("Category must not be null");
                return BadRequest("Category must not be null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for categoryDto Object");
            }

            var categoryEntity = await _repository.Category.GetCategoryAsync(id, trackChanges: true);
            if(categoryEntity == null)
            {
                _logger.LogError($"Category Name with Id : {id} not found");
                return NotFound();
            }
            _mapper.Map(categoryDto, categoryEntity);
            await _repository.SaveAsync();
            return NoContent();
        }//endMethodUpdate

        //Bedanya Patch dan Put adalah Patch bisa langsung memilih colum mana yang ingin di ubah
        //sedangkan put harus memasukkan semua column apabila tidak akan ke ubah nilainya jadi null

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCategory(int id, [FromBody]
                             JsonPatchDocument<CategoryUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError($"PatchDoc object sent is null");
                return BadRequest("PatchDoc object sent is null");
            }

            var categoryEntity = await _repository.Category.GetCategoryAsync(id, trackChanges: true);

            if (categoryEntity == null)
            {
                _logger.LogError($"Customer with id : {id} not found");
                return NotFound();
            }

            var categoryPatch = _mapper.Map<CategoryUpdateDto>(categoryEntity);

            patchDoc.ApplyTo(categoryPatch);

            TryValidateModel(categoryPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for pathc document");
                return UnprocessableEntity();
            }

            _mapper.Map(categoryPatch, categoryEntity);

            await _repository.SaveAsync();

            return NoContent();
        }//endMethodPatch

        [HttpGet("pagination")]
        public async Task<IActionResult> GetCategoryPagination([FromQuery] CategoryParameters categoryParameters)
        {
            var categoryPage = await _repository.Category.GetPaginationCategoryAsync(categoryParameters, trackChanges: false);
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categoryPage);
            return Ok(categoryDto);
        }//endMethodPagination

        [HttpGet("search")]
        public async Task<IActionResult> GetCategorySearch([FromQuery] CategoryParameters categoryParameters)
        {
            var categorySearch = await _repository.Category.GetSearchCategoryAsync(categoryParameters, trackChanges: false);
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categorySearch);
            return Ok(categoryDto);
        }//endMethodSearch

    }
}
