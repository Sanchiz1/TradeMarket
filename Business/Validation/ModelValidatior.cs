using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ModelValidatior
    {
        public static void ValidateCustomerModel(CustomerModel model)
        {
            if (model == null) throw new MarketException("Customer cannot be empty");

            if (String.IsNullOrEmpty(model.Name) ||
                String.IsNullOrEmpty(model.Surname)) throw new MarketException("Invalid name or surname");

            if (model.BirthDate <= DateTime.UtcNow.AddYears(-100) ||
                model.BirthDate >= DateTime.UtcNow.AddYears(-1)) throw new MarketException("Invalid birth date");
        }

        public static void ValidateProductModel(ProductModel model)
        {
            if (model == null) throw new MarketException("Product cannot be empty");

            if (String.IsNullOrEmpty(model.ProductName)) throw new MarketException("Invalid product name");

            if (model.Price <= 0) throw new MarketException("Price must be more than 0");
        }

        public static void ValidateProductCategoryModel(ProductCategoryModel model)
        {
            if (model == null) throw new MarketException("Category cannot be empty");

            if (String.IsNullOrEmpty(model.CategoryName)) throw new MarketException("Invalid category name");
        }
    }
}
