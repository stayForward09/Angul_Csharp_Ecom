
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackApi.Common;
using StackApi.Core.IConfiguration;
using StackApi.Data;
using StackApi.Helpers;
using StackApi.Models;

[ApiController]
[Route("api/[controller]/"), Authorize]
public class CartItemsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PartDbContext _DbContext;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IJwtService _jwtService;
    public CartItemsController(IUnitOfWork unitOfWork, IMapper mapper, PartDbContext dbContext, IWebHostEnvironment hostEnvironment, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _DbContext = dbContext;
        _hostEnvironment = hostEnvironment;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> addItemtoCart(CartItemsAdd cartItemsAdd)
    {
        var cartItem = _mapper.Map<CartItems>(cartItemsAdd);
        var user = await _jwtService.getCurrentUser(User.Identity as ClaimsIdentity);
        var data = await _unitOfWork.cartItemsRepository.getByCondition(x => x.CIPrid == cartItem.CIPrid && x.CIUsid == user.usID);
        if (data.Count() == 0)
        {
            cartItem.CIUsid = user.usID;
            cartItem.CreatedOn = DateTime.Now;
            await _unitOfWork.cartItemsRepository.Add(cartItem);
            await _unitOfWork.CompleteAsync();
        }
        return new OkObjectResult(new Response<object>() { Message = "Added", Succeeded = true });
    }

    [HttpGet, Authorize]
    [Route("[action]")]
    public async Task<IActionResult> getCartItems()
    {
        var user = await _jwtService.getCurrentUser(User.Identity as ClaimsIdentity);
        string urlpath = Request.Scheme + "://" + Request.Host.Value;
        var result = await (from Ci in _DbContext.CartItems
                            join Pa in _DbContext.Part on Ci.CIPrid equals Pa.Pid
                            where Ci.CIUsid == user.usID
                            select new
                            {
                                Cid = Ci.CITId,
                                Pid = Pa.Pid,
                                Pname = Pa.PartName,
                                Price = Pa.PartPrice,
                                Qty = Ci.CIQty,
                                Images = _DbContext.PartImages.Where(pi => !pi.PiIsTD && pi.Pid == Pa.Pid).Select(x => urlpath + Url.Content($"/PartImgs/{x.PiFilename}")).FirstOrDefault(),
                                Disount = _DbContext.Discount.Where(x => x.EndDate > DateTime.Now && x.PrdId.HasValue && (x.PrdId == Pa.Pid)).Select(x => new
                                {
                                    type = x.DType,
                                    Did = x.Did,
                                    Price = x.Amount
                                }).FirstOrDefault()
                            }).ToListAsync();
        return new OkObjectResult(new Response<object>(result));
    }

    [HttpPatch, Authorize]
    [Route("[action]")]
    public async Task<IActionResult> updateCartItems([FromBody] CartItemsAdd cartItems)
    {
        var cartItem = await _unitOfWork.cartItemsRepository.GetByID(cartItems.CITId);
        if (cartItem is not null)
        {
            _mapper.Map<CartItemsAdd, CartItems>(cartItems, cartItem);
            cartItem.UpdatedOn = DateTime.Now;
            await _unitOfWork.CompleteAsync();
        }
        return Ok(new Response<object>() { Message = "updated", Succeeded = true });
    }

    [HttpDelete, Authorize]
    [Route("[action]/{Id}")]
    public async Task<IActionResult> deleteCartItem([FromRoute] Guid Id)
    {
        await _unitOfWork.cartItemsRepository.RemoveCartItem(Id);
        await _unitOfWork.CompleteAsync();
        return Ok(new Response<object>() { Message = "deleted..", Succeeded = true });
    }
}