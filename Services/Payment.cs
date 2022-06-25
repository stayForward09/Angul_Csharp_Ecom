using System.Text;
using StackApi.Dtos;
using StackApi.Extensions;

namespace StackApi.Services;

public class Payment : IPayment
{
    private readonly HttpClient _httpClient;
    private readonly string apiCredential;
    private readonly ILogger<Payment> _logger;
    private readonly IConfiguration _configuration;
    public Payment(HttpClient httpClient, IConfiguration configuration, ILogger<Payment> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        apiCredential = configuration["RPkey"] + ":" + configuration["RPPass"];
        apiCredential = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiCredential));
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + apiCredential);
        _configuration = configuration;
    }
    public async Task<PaymentLinkRes> CreatePaymentLink(PaymentLinkReq Refs)
    {
        try
        {
            Refs.callback_url = _configuration["paymentVerifyURL"];
            Refs.callback_method = "get";
            var response = await _httpClient.PostAsJsonAsync("payment_links", Refs);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.readContentAs<PaymentLinkRes>();
                return res;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Excception : {ex.Message}");
        }
        return null;
    }

    public async Task<PaymentVerifyRes> VerifyPayment(string paymentId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"payments/{paymentId}");
            if (response.IsSuccessStatusCode)
            {
                var resData = await response.readContentAs<PaymentVerifyRes>();
                return resData;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Excception : {ex.Message}");
        }
        return null;
    }
}

// http://localhost:4200/verifyPayment?razorpay_payment_id=pay_Jkr2RUAaqjTH2q&razorpay_payment_link_id=plink_Jkr0VWiUbVynGj&razorpay_payment_link_reference_id=&razorpay_payment_link_status=paid&razorpay_signature=817b14fbe333e03b4ad251ff611ac44dabeb12ef7c7434acd4994674bc0e5059