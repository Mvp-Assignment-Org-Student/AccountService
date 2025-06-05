namespace Business.Models;

public class AccountServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }

    public string? Token { get; set; }
}


public class AccountServiceResult<T> : AccountServiceResult
{
   public T? Result { get; set; }
}

