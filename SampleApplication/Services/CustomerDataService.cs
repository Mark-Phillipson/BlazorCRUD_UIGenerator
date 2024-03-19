using Ardalis.GuardClauses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Repositories;
using SampleApplication.DTOs;


namespace SampleApplication.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerDataService(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }
        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var Customers = await _customerRepository.GetAllCustomersAsync(300);
            return Customers.ToList();
        }
        public async Task<List<CustomerDTO>> SearchCustomersAsync(string serverSearchTerm)
        {
            var Customers = await _customerRepository.SearchCustomersAsync(serverSearchTerm);
            return Customers.ToList();
        }

        public async Task<CustomerDTO?> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return customer;
        }
        public async Task<CustomerDTO?> AddCustomer(CustomerDTO customerDTO)
        {
            Guard.Against.Null(customerDTO);
            var result = await _customerRepository.AddCustomerAsync(customerDTO);
            if (result == null)
            {
                throw new Exception($"Add of customer failed ID: {customerDTO.id}");
            }
            return result;
        }
        public async Task<CustomerDTO> UpdateCustomer(CustomerDTO customerDTO, string? username)
        {
            Guard.Against.Null(customerDTO);
            Guard.Against.Null(username);
            var result = await _customerRepository.UpdateCustomerAsync(customerDTO);
            if (result == null)
            {
                throw new Exception($"Update of customer failed ID: {customerDTO.id}");
            }
            return result;
        }

        public async Task DeleteCustomer(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}