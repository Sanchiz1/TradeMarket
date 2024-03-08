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
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        // GET: api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            return Ok(await _receiptService.GetAllAsync());
        }

        //GET: api/receipts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);

            if (receipt == null) return NotFound("Receipt not found");

            return Ok(receipt);
        }

        //GET: api/receipts/1/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetDetailsById(int id)
        {
            var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);

            return Ok(receiptDetails);
        }

        //GET: api/receipts/1/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetSumById(int id)
        {
            var sum = await _receiptService.ToPayAsync(id);

            return Ok(sum);
        }

        //GET: api/receipts/period?startDate=2021-12-1&endDate=2020-12-31
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetSumById(DateTime startDate, DateTime endDate)
        {
            var sum = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);

            return Ok(sum);
        }

        // POST: api/receipts
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReceiptModel value)
        {
            await _receiptService.AddAsync(value);

            return Ok(value);
        }

        // PUT: api/receipts/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ReceiptModel value)
        {
            value.Id = id;

            await _receiptService.UpdateAsync(value);

            return Ok(value);
        }

        // PUT: api/receipts/1/products/add/1/2
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProduct(int id, int productId, int quantity)
        {
            await _receiptService.AddProductAsync(productId, id, quantity);

            return Ok();
        }

        // PUT: api/receipts/1/products/remove/1/2
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProduct(int id, int productId, int quantity)
        {
            await _receiptService.RemoveProductAsync(productId, id, quantity);

            return Ok();
        }

        // PUT: api/receipts/1/checkout 
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> Checkout(int id)
        {
            await _receiptService.CheckOutAsync(id);

            return Ok();
        }

        // DELETE: api/receipts/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _receiptService.DeleteAsync(id);

            return Ok();
        }
    }
}
