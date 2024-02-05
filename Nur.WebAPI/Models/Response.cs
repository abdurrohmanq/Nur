namespace Nur.WebAPI.Models;

public class Response
{
    public int Status { get; set; } = 200;
    public string Message { get; set; } = "Success";
    public object Data { get; set; } = default!;
}
