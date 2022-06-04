namespace StackApi.Helpers;

public class Response<T>
{
    public Response()
    {

    }
    public Response(T data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }
    public Response(string message, bool issucceed)
    {
        Succeeded = issucceed;
        Message = message;
        Errors = null;
    }
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
}