using Application.ROP;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Immutable;
using System.Net;

namespace Application.Extensions;

public static class ValidatorExtension
{
    extension<T>(IValidator<T> validator)
    {
        public async Task<Result<T>> ValidateAndMap(T command)
        {
            ValidationResult result = await validator.ValidateAsync(command);
            if(result.IsValid) return Result.Success(command);

            var errors = result.Errors
                .Select(e => new Error(e.ErrorCode, e.ErrorMessage))
                .ToImmutableArray();

            return Result.Failure<T>(HttpStatusCode.BadRequest, errors);
        }
    }
}
