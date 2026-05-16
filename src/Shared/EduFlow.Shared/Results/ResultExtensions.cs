using Microsoft.AspNetCore.Http;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace EduFlow.Shared.Results;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return HttpResults.Ok(result.Data);

        return result.Error!.Status switch
        {
            404 => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 404,
                type: result.Error.Type),
            409 => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 409,
                type: result.Error.Type),
            400 => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 400,
                type: result.Error.Type),
            _ => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 500,
                type: result.Error.Type)
        };
    }

    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
            return HttpResults.NoContent();

        return result.Error!.Status switch
        {
            404 => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 404,
                type: result.Error.Type),
            400 => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 400,
                type: result.Error.Type),
            _ => HttpResults.Problem(
                detail: result.Error.Detail,
                title: result.Error.Title,
                statusCode: 500,
                type: result.Error.Type)
        };
    }
}
