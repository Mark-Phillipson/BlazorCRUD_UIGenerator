
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public CustomerRepository(IDbContextFactory<MyDbContext> contextFactory,IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
		        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync(int maxRows= 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var Customers= await context.Customers
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<CustomerDTO> CustomersDTO = _mapper.Map<List<Customer>, IEnumerable<CustomerDTO>>(Customers);
            return CustomersDTO;
        }
        public async Task<IEnumerable<CustomerDTO>> SearchCustomersAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var Customers= await context.Customers
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<CustomerDTO> CustomersDTO = _mapper.Map<List<Customer>, IEnumerable<CustomerDTO>>(Customers);
            return CustomersDTO;
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result =await context.Customers.AsNoTracking()
              .FirstOrDefaultAsync(c => c.id == id);
            if (result == null) return null;
            CustomerDTO customerDTO=_mapper.Map<Customer,CustomerDTO>(result);
            return customerDTO;
        }

        public async Task<CustomerDTO?> AddCustomerAsync(CustomerDTO customerDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            Customer customer = _mapper.Map<CustomerDTO, Customer>(customerDTO);
            var addedEntity = context.Customers.Add(customer);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            CustomerDTO resultDTO=_mapper.Map<Customer, CustomerDTO>(customer);
            return resultDTO;
        }

        public async Task<CustomerDTO?> UpdateCustomerAsync(CustomerDTO customerDTO)
        {
            Customer customer=_mapper.Map<CustomerDTO, Customer>(customerDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundCustomer = await context.Customers.AsNoTracking().FirstOrDefaultAsync(e => e.id == customer.id);

                if (foundCustomer != null)
                {
                    var mappedCustomer = _mapper.Map<Customer>(customer);
                    context.Customers.Update(mappedCustomer);
                    await context.SaveChangesAsync();
                    CustomerDTO resultDTO = _mapper.Map<Customer, CustomerDTO>(mappedCustomer);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteCustomerAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundCustomer = context.Customers.FirstOrDefault(e => e.id == id);
            if (foundCustomer == null)
            {
                return;
            }
            context.Customers.Remove(foundCustomer);
            await context.SaveChangesAsync();
        }
    }
}