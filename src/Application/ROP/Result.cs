using System.Collections.Immutable;
using System.Net;

namespace Application.ROP
{
    public readonly struct Result<T>(T? value, ImmutableArray<Error> errors, HttpStatusCode statusCode)
    {
        public T? Value { get; } = value;
        public ImmutableArray<Error> Errors { get; } = errors;
        public bool Succeeded => Errors.IsEmpty;
        public HttpStatusCode StatusCode { get; } = statusCode;
    }

    public record Error(string Code, string Message);

    public readonly struct Unit
    {
        public static readonly Unit Value = new();
    }

    public static class Result
    {
        public static Result<T> Success<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("El código de estado de éxito debe ser menor a 400.", nameof(statusCode));
            }

            return new(value, [], statusCode);
        }

        public static Result<Unit> Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
        {
            return Success(Unit.Value, statusCode);
        }

        public static Result<T> Failure<T>(HttpStatusCode statusCode, ImmutableArray<Error> errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("El código de estado de error debe ser igual o mayor a 400.", nameof(statusCode));
            }

            if (errors.Length == 0)
            {
                throw new ArgumentException("Debe proporcionarse al menos 1 error.", nameof(errors));
            }

            return new(default, errors, statusCode);
        }

        public static Result<T> Failure<T>(HttpStatusCode statusCode, params Error[] errors)
        {
            return Failure<T>(statusCode, errors.ToImmutableArray());
        }
    }
}
