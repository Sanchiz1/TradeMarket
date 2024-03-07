using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ProductModel>>(await _unitOfWork.ProductRepository.GetAllWithDetailsAsync());
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(await _unitOfWork.ProductCategoryRepository.GetAllAsync());
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = _mapper.Map<IEnumerable<ProductModel>>(await _unitOfWork.ProductRepository.GetAllWithDetailsAsync());


            if (filterSearch.CategoryId != null) products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId);

            if (filterSearch.MinPrice != null) products = products.Where(p => p.Price >= filterSearch.MinPrice);

            if (filterSearch.MaxPrice != null) products = products.Where(p => p.Price <= filterSearch.MaxPrice);

            return products;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            return _mapper.Map<ProductModel>(await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task AddAsync(ProductModel model)
        {
            ModelValidatior.ValidateProductModel(model);

            await _unitOfWork.ProductRepository.AddAsync(_mapper.Map<Product>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ModelValidatior.ValidateProductCategoryModel(categoryModel);

            await _unitOfWork.ProductCategoryRepository.AddAsync(_mapper.Map<ProductCategory>(categoryModel));

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ModelValidatior.ValidateProductModel(model);

            _unitOfWork.ProductRepository.Update(_mapper.Map<Product>(model));

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ModelValidatior.ValidateProductCategoryModel(categoryModel);

            _unitOfWork.ProductCategoryRepository.Update(_mapper.Map<ProductCategory>(categoryModel));

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);

            await _unitOfWork.SaveAsync();
        }
    }
}
