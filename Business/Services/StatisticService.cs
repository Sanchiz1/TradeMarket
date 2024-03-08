using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            receipts = receipts.Where(r => r.CustomerId == customerId);

            var receiptDetails = receipts.SelectMany(r => r.ReceiptDetails);

            var products = receiptDetails.GroupBy(r => r.Product).OrderByDescending(r => r.Sum(r => r.Quantity)).Select(g => g.Key).Take(productCount);

            return _mapper.Map<IEnumerable<ProductModel>>(
                (products));
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            receipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate && r.ReceiptDetails != null);

            var receiptDetails = receipts.SelectMany(r => r.ReceiptDetails);

            receiptDetails = receiptDetails.Where(rd => rd.Product.ProductCategoryId == categoryId);

            return receiptDetails.Sum(rd => rd.DiscountUnitPrice * rd.Quantity);
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();


            var products = receiptDetails.GroupBy(r => r.Product).OrderByDescending(r => r.Sum(r => r.Quantity)).Select(g => g.Key).Take(productCount);

            return _mapper.Map<IEnumerable<ProductModel>>(
                (products));
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            receipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

            var customers = receipts.GroupBy(r => r.Customer)
                .OrderByDescending(r => r
                    .Sum(r => r.ReceiptDetails
                    .Sum(r => r.Quantity * r.DiscountUnitPrice)))
                .Select(g => 
                new CustomerActivityModel
                {
                    CustomerId = g.Key.Id,
                    CustomerName = g.Key.Person.Name + " " + g.Key.Person.Surname,
                    ReceiptSum = g.Sum(r => r.ReceiptDetails
                    .Sum(r => r.Quantity * r.DiscountUnitPrice))
                }).Take(customerCount);


            return customers;
        }
    }
}
