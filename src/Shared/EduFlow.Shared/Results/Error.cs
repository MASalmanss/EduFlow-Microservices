namespace EduFlow.Shared.Results;

// RFC 7807 — Problem Details
public record Error(
    string Type,
    string Title,
    int Status,
    string Detail)
{
    public static Error NotFound(string detail) => new(
        "https://tools.ietf.org/html/rfc7807#section-3",
        "Not Found",
        404,
        detail);

    public static Error Conflict(string detail) => new(
        "https://tools.ietf.org/html/rfc7807#section-3",
        "Conflict",
        409,
        detail);

    public static Error Validation(string detail) => new(
        "https://tools.ietf.org/html/rfc7807#section-3",
        "Validation Error",
        400,
        detail);

    public static Error Unexpected(string detail) => new(
        "https://tools.ietf.org/html/rfc7807#section-3",
        "Internal Server Error",
        500,
        detail);
}
