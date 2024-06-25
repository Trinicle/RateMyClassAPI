namespace RateMyClass.API.Models.Response
{
    public class GetMultipleResponse<T>
    {
        public int count { get; set; }
        public IEnumerable<T> result { get; set; } = Enumerable.Empty<T>();
    }
}
