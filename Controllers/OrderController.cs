using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackApi.Core.IConfiguration;
using StackApi.Data;
using StackApi.Dtos;
using StackApi.Helpers;
using StackApi.Models;
using StackApi.Services;
using StackApi.Common;
using System.Security.Claims;

namespace StackApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitofWork;
    private readonly IMapper _mapper;
    private readonly IPayment _payment;
    private readonly PartDbContext _dbContext;
    private readonly IJwtService _jwtService;
    private readonly ILogger<OrderController> _logger;
    public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IPayment payment,
     PartDbContext dbContext, IJwtService jwtService, ILogger<OrderController> logger)
    {
        _unitofWork = unitOfWork;
        _mapper = mapper;
        _payment = payment;
        _dbContext = dbContext;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost, Authorize]
    [Route("[action]")]
    public async Task<IActionResult> createOrder([FromBody] List<createOrderReq> createOrders, int orderType)
    {
        var user = await _jwtService.getCurrentUser(User.Identity as ClaimsIdentity); // get auth user details
        var cartItems = createOrders.Select(x => x.Cid).ToList();

        var (parts, res) = await getQueryPaymentLinkCreation(createOrders, orderType); // create payment link and fetch order related data
        if (parts is null || parts?.Count() <= 0)  // if no part found
        {
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Bad Data" });
        }
        if (res is null && orderType == 0) // online payment failure result
        {
            return new OkObjectResult(new Response<object>() { Succeeded = false, Message = "Failed to Create Order." });
        }

        bool orderCreated = await saveOrderDetails(parts, res, user, orderType, cartItems); // save orders,OrderItems,OrderDiscount
        if (!orderCreated) // if exception occurs
        {
            return new OkObjectResult(new Response<object>() { Succeeded = false, Message = "Failed to Create Order." });
        }
        return Ok(new Response<object>() { Succeeded = true, Message = "Order Created, waiting for payment", Data = res });
    }

    [HttpGet, Authorize]
    [Route("[action]/{Id}/{paymentLinkRef}")]
    public async Task<IActionResult> verifyPayment([FromRoute] string Id, string paymentLinkRef)
    {
        var order = await _unitofWork.ordersRepositories.getFirstByCondition(x => x.PayRef == paymentLinkRef || x.PayRef == Id);
        if (order is null)
        {
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Invalid Order" });
        }
        if (order.PayStatus == 1)
        {
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Order Already Placed" });
        }

        var data = await _payment.VerifyPayment(Id);
        order.PayRef = Id;
        order.PayStatus = data.status == "captured" ? 1 : 2;
        await _unitofWork.CompleteAsync();
        return Ok(new Response<PaymentVerifyRes>(data));
    }

    [HttpGet, Authorize]
    [Route("[action]")]
    public async Task<IActionResult> getOrders()
    {
        string urlpath = Request.Scheme + "://" + Request.Host.Value;
        var user = await _jwtService.getCurrentUser(User.Identity as ClaimsIdentity);
        var Orders = await _unitofWork.ordersRepositories.getOrderbyCondition(x => x.UsId == user.usID);
        var result = (from order in Orders
                      select new
                      {
                          OrderId = order.Oid,
                          Address = order.Address,
                          TotalPrice = order.TotalPrice,
                          OrderItems = order.OrderItems.Select(x => new
                          {
                              Id = x.OIid,
                              Qty = x.Qty,
                              OrderPrice = x.OrderPrice,
                              ListPrice = x.ListPrice,
                              Part = new
                              {
                                  Part = x.Part.PartName,
                                  id = x.Part.Pid,
                                  images = x.Part.PartImages.Where(p => p.PiIsTD == false).Select(p => urlpath + Url.Content($"/PartImgs/{p.PiFilename}")).FirstOrDefault()
                              },
                              Discounts = x.OrdersDiscount != null ? new { x.OrdersDiscount.Amount, x.OrdersDiscount.DType, x.OrdersDiscount.CouponName } : null
                          }).ToList()
                      }).ToList();
        return Ok(new Response<object>(result));
    }

    [NonAction]
    public decimal getDiscountPrice(decimal price, decimal disocuntAmount, int Qty)
    {
        decimal discountPrice = 0;
        var per = (disocuntAmount / 100) * price;
        discountPrice = price - per;
        return discountPrice * Qty;
    }

    [NonAction]
    public async Task<(List<orderCreateQuery>, PaymentLinkRes)> getQueryPaymentLinkCreation(List<createOrderReq> createOrders, int orderType)
    {
        var parts = (from or in createOrders
                     join p in _dbContext.Part on or.prdId equals p.Pid
                     join d in _dbContext.Discount on or.DId equals d.Did into hj
                     from dis in hj.DefaultIfEmpty()
                     select new orderCreateQuery
                     {
                         pid = p.Pid,
                         listPrice = p.PartPrice,
                         Discount = dis,
                         orderPrice = getDiscountPrice(p.PartPrice, dis?.Amount ?? 0, or.Qty),
                         Qty = or.Qty
                     }).ToList();

        if (orderType == 0)
        {
            var notes = new notes();
            var paymentRefs = new PaymentLinkReq();
            notes.refIds.AddRange(createOrders.Select(x => x.prdId.ToString()));
            paymentRefs.notes = notes;
            paymentRefs.amount = 239200; // Convert.ToInt32((orders.TotalPrice / 10000));
            var res = await _payment.CreatePaymentLink(paymentRefs);

            return (parts, res);
        }
        else
        {
            return (parts, null);
        }
    }

    [NonAction]
    public async Task<bool> saveOrderDetails(List<orderCreateQuery> parts, PaymentLinkRes res, TokenUserDetails user, int payType, List<Guid> cartItems)
    {
        try
        {
            var uDetails = await _unitofWork.userDetailsRepository.getFirstByCondition(x => x.UsId == user.usID);
            Orders orders = new Orders();
            orders.Address = uDetails.Address;
            orders.TotalPrice = parts.Sum(x => x.orderPrice);
            orders.PayType = payType;
            orders.PayStatus = 0;
            orders.UsId = user.usID;
            orders.PayRef = payType == 0 ? res.id : "COD";

            await _unitofWork.ordersRepositories.Add(orders);
            foreach (var x in parts)
            {
                var OrderItem = new OrderItems()
                {
                    Oid = orders.Oid,
                    ListPrice = x.listPrice,
                    OrderPrice = x.orderPrice,
                    Prid = x.pid,
                    Qty = x.Qty
                };
                await _unitofWork.orderItemsRepository.Add(OrderItem);

                if (x.Discount is not null)
                {
                    var discountOrder = new OrdersDiscount()
                    {
                        OIid = OrderItem.OIid,
                        Amount = x.Discount.Amount,
                        CouponCode = x.Discount.CouponCode,
                        CouponName = x.Discount.CouponName,
                        DType = x.Discount.DType
                    };
                    await _unitofWork.ordersDiscountRepository.Add(discountOrder);
                }
            }
            foreach (Guid id in cartItems)
            {
                await _unitofWork.cartItemsRepository.RemoveCartItem(id);
            }
            await _unitofWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return false;
    }
}