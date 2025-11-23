using System.Net;

namespace Application.ROP
{
    public static class RailwayExtension
    {
        extension<TIn>(Task<Result<TIn>> currentTask)
        {
            public async Task<Result<TOut>> Bind<TOut>(Func<TIn, Task<Result<TOut>>> next)
            {
                Result<TIn> result = await currentTask;

                return result.Succeeded
                    ? await next(result.Value!)
                    : Result.Failure<TOut>(result.StatusCode, result.Errors);
            }

            public async Task<Result<TOut>> Map<TOut>(Func<TIn, Task<TOut>> next)
            {
                return await currentTask.Bind(async input => Result.Success(await next(input)));
            }

            public async Task<Result<TIn>> Then(Func<TIn, Task> next)
            {
                return await currentTask.Bind(async input =>
                {
                    await next(input);
                    return Result.Success(input);
                });
            }

            public async Task<Result<TIn>> WithStatusCode(HttpStatusCode statusCode)
            {
                return await currentTask.Bind(async input =>
                {
                    return Result.Success(input, statusCode);
                });
            }
        }
    }
}
