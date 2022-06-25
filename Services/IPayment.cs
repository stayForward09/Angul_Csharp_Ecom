using StackApi.Dtos;

namespace StackApi.Services;

public interface IPayment
{
    Task<PaymentLinkRes> CreatePaymentLink(PaymentLinkReq Refs);
    Task<PaymentVerifyRes> VerifyPayment(string paymentId);
}