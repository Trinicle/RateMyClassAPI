namespace RateMyClass.API.Models.Response
{
    public class GetSingleResponse<T>
    {
        public T result { get; set; } = default!;
    }
}
