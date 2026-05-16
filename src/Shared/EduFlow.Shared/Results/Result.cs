namespace EduFlow.Shared.Results;

public class Result<T>
{
    public T? Data { get; }
    public Error? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(T data) => Data = data;
    private Result(Error error) => Error = error;

    public static Result<T> Success(T data) => new(data);
    public static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T data) => Success(data);
    public static implicit operator Result<T>(Error error) => Failure(error);
}

public class Result
{
    public Error? Error { get; }
    public bool IsSuccess => Error is null;

    private Result() { }
    private Result(Error error) => Error = error;

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);

    public static implicit operator Result(Error error) => Failure(error);
}
