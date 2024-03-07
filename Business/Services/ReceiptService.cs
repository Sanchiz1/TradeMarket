using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ReceiptModel>>(await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync());
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            return _mapper.Map<ReceiptModel>(await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>(
                (await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return _mapper.Map<IEnumerable<ReceiptModel>>(
                (await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate));
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            return (await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId))
                .ReceiptDetails.Sum(r => r.DiscountUnitPrice * r.Quantity);
        }

        public async Task AddAsync(ReceiptModel model)
        {
            await _unitOfWork.ReceiptRepository.AddAsync(_mapper.Map<Receipt>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);

            receipt.IsCheckedOut = true;

            _unitOfWork.ReceiptRepository.Update(receipt);

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            _unitOfWork.ReceiptRepository.Update(_mapper.Map<Receipt>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receipt == null) throw new MarketException("Receipt does not exists");

            if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Any(r => r.ProductId == productId))
            {
                var receiptDetails = receipt.ReceiptDetails.First(r => r.ProductId == productId);

                receiptDetails.Quantity += quantity;

                _unitOfWork.ReceiptDetailRepository.Update(receiptDetails);

                await _unitOfWork.SaveAsync();

                return;
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product == null) throw new MarketException("Product does not exists");

            await _unitOfWork.ReceiptDetailRepository.AddAsync(new ReceiptDetail
            {
                ReceiptId = receiptId,
                ProductId = productId,
                UnitPrice = product.Price,
                DiscountUnitPrice = product.Price - product.Price * receipt.Customer.DiscountValue / 100,
                Quantity = quantity
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receipt == null) throw new MarketException("Receipt does not exists");

            if (receipt.ReceiptDetails == null || !receipt.ReceiptDetails.Any(r => r.ProductId == productId))
                throw new MarketException("Receipt does not have this product");

            var receiptDetails = receipt.ReceiptDetails.First(r => r.ProductId == productId);

            receiptDetails.Quantity -= quantity;

            if (receiptDetails.Quantity == 0)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetails);
            }
            else
            {
                _unitOfWork.ReceiptRepository.Update(receipt);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);

            if (receipt == null) throw new MarketException("Receipt does not exists");

            foreach (var details in receipt.ReceiptDetails)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(details);
            }

            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }
    }
}
