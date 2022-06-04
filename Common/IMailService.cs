namespace StackApi.Common;

public interface IMailService
{
    Task<bool> SendEmail(string tomailid,string subject,string content);
    string GenerateOTP();
    Task<string> CreateContent(string name,string OTP);
}