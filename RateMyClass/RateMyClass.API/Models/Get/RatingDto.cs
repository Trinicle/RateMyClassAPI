namespace RateMyClass.API.Models.Get
{
    public class RatingDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quality { get; set; }
        public int Difficulty { get; set; }
        public bool TakeAgain { get; set; }
        public DateTime Date { get; set; }
    }
}
