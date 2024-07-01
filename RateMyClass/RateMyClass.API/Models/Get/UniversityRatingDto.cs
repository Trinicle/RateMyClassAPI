namespace RateMyClass.API.Models.Get
{
    public class UniversityRatingDto
    {
        public int Id { get; set; }
        public int Quality { get; set; }
        public int Location { get; set; }
        public int Opportunities { get; set; }
        public int Facilities { get; set; }
        public int Internet { get; set; }
        public int Food { get; set; }
        public int Clubs { get; set; }
        public int Social { get; set; }
        public int Happiness { get; set; }
        public int Safety { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

    }
}
