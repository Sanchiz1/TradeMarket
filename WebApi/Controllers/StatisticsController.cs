using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        // GET: api/statistic/popularProducts?productCount=2
        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts(int productCount)
        {
            return Ok(await _statisticService.GetMostPopularProductsAsync(productCount));
        }

        // GET: api/statisic/customer/1/3
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int id, int productCount)
        {
            return Ok(await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id));
        }

        // GET: api/statistic/activity/2?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostValuableCustomers(int customerCount, DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate));
        }

        // GET: api/statistic/income/2?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate));
        }
    }
}
