using SharedKernel.SeedWorks;

namespace SharedKernel.Utils;

public static class ResponseFactory
{
    private static Response<T> ToResponse<T>(T? data, string message)
    {
        return new Response<T>(data, message);
    }

    private static Response ToResponse(string message)
    {
        return new Response(message);
    }

    public static Response<T> Ok<T>(T? value, string message = "Ok")
    {
        return ToResponse(value, message);
    }

    public static Response Ok(string message = "Ok")
    {
        return ToResponse(message);
    }

    public static Response<T> Created<T>(T? value, string message = "Created")
    {
        return ToResponse(value, message);
    }

    public static Response Created(string message = "Created")
    {
        return ToResponse(message);
    }

    public static Response NoContent(string message = "No Content")
    {
        return ToResponse(message);
    }
}