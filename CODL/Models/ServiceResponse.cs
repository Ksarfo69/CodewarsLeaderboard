using System.Text.Json;

namespace CODL.Models;

public class ServiceResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T Data { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}