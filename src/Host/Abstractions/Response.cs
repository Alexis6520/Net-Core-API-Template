using Application.ROP;

namespace Host.Abstractions
{
    public class Response<T>
    {
        public T? Value { get; set; }
        public IEnumerable<Error> Errors { get; set; } = [];
    }
}
