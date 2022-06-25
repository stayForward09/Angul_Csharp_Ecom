using StackApi.Models;

namespace StackApi.Dtos;

public class rzPaymentModels
{

}

public class PaymentLinkReq
{
    public int amount { get; set; }
    public bool accept_partial = false;
    public string description { get; set; }
    public bool reminder_enable = true;
    public notes notes { get; set; }
    public string callback_url { get; set; }
    public string callback_method { get; set; }
}

public class notes
{
    public List<string> refIds { get; set; } = new List<string>();
}

public class notify
{
    public bool sms = true;
    public bool email = true;
}

public class PaymentLinkRes
{
    public string short_url { get; set; }
    public string id { get; set; }
}

public class PaymentVerifyRes
{
    public string id { get; set; }
    public string status { get; set; }
}

public class orderCreateQuery
{
    public Guid pid { get; set; }
    public decimal listPrice { get; set; }
    public Discount Discount { get; set; }
    public decimal orderPrice { get; set; }
    public int Qty { get; set; }
}