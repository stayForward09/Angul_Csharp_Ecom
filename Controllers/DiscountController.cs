using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackApi.Core.IConfiguration;
using StackApi.Dtos;
using StackApi.Helpers;
using StackApi.Models;

namespace StackApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class DiscountController : ControllerBase
{
    private readonly IUnitOfWork _unitofWork;
    private readonly IMapper _mapper;
    public DiscountController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitofWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var data = await _unitofWork.discountRepository.All();
        return Ok(data);
    }

    [HttpPost, Authorize(Policy = "Admin")]
    [Route("[action]")]
    public async Task<IActionResult> createDiscount([FromBody] DiscountAdd discountAdd)
    {
        var discountExists = await _unitofWork.discountRepository.fetchDiscountbyCondition(x => x.EndDate > discountAdd.StartDate && (x.PrdId == discountAdd.PrdId || x.CId == discountAdd.CId));
        if (discountExists is not null)
        {
            string type = discountAdd.PrdId is null ? "Category" : "Product";
            return BadRequest(new Response<object>() { Succeeded = false, Message = $"Discount already Exists for {type}" });
        }
        if (discountAdd.DType == 2 && discountAdd.Amount > 100)
        {
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Discount Percentage Should not greater than 100" });
        }
        var discount = _mapper.Map<Discount>(discountAdd);
        try
        {
            await _unitofWork.discountRepository.Add(discount);
            await _unitofWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Succeeded = false, Errors = new string[] { ex.Message } });
        }
        return Ok(new Response<object>() { Succeeded = true, Data = discount });
    }

    [HttpPut, Authorize(Policy = "Admin")]
    [Route("[action]/{id}")]
    public async Task<IActionResult> updateDiscount([FromRoute] Guid id, [FromBody] DiscountAdd discountAdd)
    {
        var dbData = await _unitofWork.discountRepository.fetchDiscountbyCondition(x => x.Did == id, true);
        if (dbData is null)
        {
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Discount Not Found" });
        }
        _mapper.Map<DiscountAdd, Discount>(discountAdd, dbData);
        await _unitofWork.CompleteAsync();
        return Ok(new Response<object>() { Succeeded = true, Message = "Updated" });
    }
}