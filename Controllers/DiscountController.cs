using AutoMapper;
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

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateDiscount([FromBody] DiscountAdd discountAdd)
    {
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
}