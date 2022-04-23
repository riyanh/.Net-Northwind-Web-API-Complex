using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using AutoMapper;
using Northwind.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Northwind.Entities.RequestFeatures;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CustomerController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerAsync()
        {
            var customer = await _repository.Customers.GetAllCustomerAsync(trackChanges: false);
            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customer);
            return Ok(customerDto);
        }

        [HttpGet("{custId}", Name = "CustomerById")]
        public async Task<IActionResult> GetCustomer(string custId)
        {
            var customer = await _repository.Customers.GetCustomerAsync(custId, false);
            if(customer == null)
            {
                _logger.LogInfo($"Customer with id : {custId} not found");
                return NotFound();
            }
            else
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerDto customerDto)
        {
            if(customerDto == null)
            {
                _logger.LogError("Customer is null");
                return BadRequest("Customer is null");
            }

            //object modelState digunakan untuk validasi data yang ditangkap oleh customerDto
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid modelstate customerdto");
                return UnprocessableEntity(ModelState);
            }

            var customerEntity = _mapper.Map<Customer>(customerDto);
            _repository.Customers.CreateCustomerAsync(customerEntity);
            await _repository.SaveAsync(); 

            var customerResult = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { custId = customerResult.CustomerId }, customerResult);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _repository.Customers.GetCustomerAsync(id, trackChanges: false);
            if (customer == null)
            {
                _logger.LogInfo($"Customer with id : {id} doesn't exist in database");
                return NotFound();
            }

            _repository.Customers.DeleteCustomerAsync(customer);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{custId}")]
        public async Task<IActionResult> UpdateCustomer(string custId, [FromBody] CustomerUpdateDto customerDto)
        {
            if (customerDto == null)
            {
                _logger.LogError("Customer must not be null");
                return BadRequest("Customer must not be null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for customerdto object");
                return UnprocessableEntity(ModelState);
            }
            var customerEntity = await _repository.Customers.GetCustomerAsync(custId, trackChanges: true);

            if (customerEntity == null)
            {
                _logger.LogError($"Company with id : {custId} not found");
                return NotFound();
            }

            _mapper.Map(customerDto, customerEntity);
            //_repository.Customer.UpdateCustomer(customerEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCustomer(string id, [FromBody]
                             JsonPatchDocument<CustomerUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError($"PatchDoc object sent is null");
                return BadRequest("PatchDoc object sent is null");
            }

            var customerEntity = await _repository.Customers.GetCustomerAsync(id, trackChanges: true);

            if (customerEntity == null)
            {
                _logger.LogError($"Customer with id : {id} not found");
                return NotFound();
            }

            var customerPatch = _mapper.Map<CustomerUpdateDto>(customerEntity);

            patchDoc.ApplyTo(customerPatch);

            TryValidateModel(customerPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for pathc document");
                return UnprocessableEntity();
            }

            _mapper.Map(customerPatch, customerEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery]CustomerParameters customerParameters)
        {
            var customerPage = await _repository.Customers.GetPaginationCustomerAsync(customerParameters, trackChanges: false);
            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customerPage);
            return Ok(customerDto);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetCustomerSearch([FromQuery] CustomerParameters customerParameters)
        {
            var customerSearch = await _repository.Customers.SearchCustomer(customerParameters, trackChanges: false);
            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customerSearch);
            return Ok(customerDto);
        }

    }//endClass
}
