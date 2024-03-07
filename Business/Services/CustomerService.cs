using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CustomerModel>>(await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync());
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            return _mapper.Map<CustomerModel>(await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            return _mapper.Map< IEnumerable<CustomerModel>>(
                (await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync())
                .Where(c => c.Receipts
                    .Any(r => r.ReceiptDetails
                        .Any(rd => rd.ProductId == productId))));
        }

        public async Task AddAsync(CustomerModel model)
        {
            ModelValidatior.ValidateCustomerModel(model);

            await _unitOfWork.CustomerRepository.AddAsync(_mapper.Map<Customer>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            ModelValidatior.ValidateCustomerModel(model);

            _unitOfWork.CustomerRepository.Update(_mapper.Map<Customer>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }
    }
}
