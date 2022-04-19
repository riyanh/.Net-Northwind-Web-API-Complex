using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using AutoMapper;
using Northwind.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult GetCustomers()
        {
            try
            {
                var customers = _repository.Customers.GetAllCustomer(trackChanges: false);
                /*var customerDto = customers.Select(c => new CustomerDto
                {
                    CustomerID = c.CustomerId,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName

                }).ToList();*/
                //var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCustomers)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "CustomerById")]
        public IActionResult GetCustomer(string id)
        {
            var customer = _repository.Customers.GetCustomer(id, trackChanges: false);
            if(customer == null)
            {
                _logger.LogInfo($"Customer with Id : {id} doesn't exist");
                return NotFound();
            }
            else
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if(customerDto == null)
            {
                _logger.LogError("Customer object is null");
                return BadRequest("Customer object is null");
            }

            var customerEntity = _mapper.Map<Customer>(customerDto);
            _repository.Customers.CreateCustomer(customerEntity);
            _repository.Save();

            var customerResult = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { id = customerResult.CustomerID }, customerResult);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            var customer = _repository.Customers.GetCustomer(id, trackChanges: false);
            if(customer == null)
            {
                _logger.LogInfo($"Customer With Id : {id} not found");
                return NotFound();
            }

            _repository.Customers.DeleteCustomer(customer);
            _repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(string id, [FromBody] CustomerDto customerDto)
        {
            if(customerDto == null)
            {
                _logger.LogError($"Customer Must not be null");
                return BadRequest("Customer Must not be null");
            }

            var customerEntity = _repository.Customers.GetCustomer(id, trackChanges: true);
            if(customerEntity == null)
            {
                _logger.LogInfo($"Customer with id : {id} not found");
                return NotFound();
            }

            _mapper.Map(customerDto, customerEntity);
            _repository.Customers.UpdateCustomer(customerEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
