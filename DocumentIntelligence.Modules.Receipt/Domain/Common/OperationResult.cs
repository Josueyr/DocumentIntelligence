// Contenedor genérico para cualquier resultado de operación
public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public T? Data { get; }

    private OperationResult(T? data)
    {
        IsSuccess = true;
        Data = data;
        ErrorMessage = null;
    }

    private OperationResult(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
        Data = default;
    }

    public static OperationResult<T> Success(T data) => new(data);
    public static OperationResult<T> Failure(string errorMessage) => new(errorMessage);
}
