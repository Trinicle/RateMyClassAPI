using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public class UniversityRatingInfoRepository : IUniversityRatingInfoRepository
    {
        private readonly UniversityInfoContext _context;

        public UniversityRatingInfoRepository(UniversityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddRatingForUniversity(University university, UniversityRating rating)
        {
            university.Ratings.Add(rating);

            await SaveChanges();

            return await RatingExists(rating.Id);
        }

        public async Task<bool> DeleteRating(University university, int ratingId)
        {
            var rating = await GetRatingForUniversityById(university.Id, ratingId);

            if (rating is null)
            {
                return false;
            }

            university.Ratings.Remove(rating);

            await SaveChanges();

            return !await RatingExists(ratingId);
        }

        public async Task<UniversityRating?> GetRatingForUniversityById(int universityId, int ratingId)
        {
            return await _context.UniversityRatings
                .Where(r => r.UniversityId == universityId && r.Id == ratingId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UniversityRating>> GetRatingsForUniversity(int universityId, int amount)
        {
            return await _context.UniversityRatings
                .Where(r => r.UniversityId == universityId)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<bool> RatingExists(int ratingId)
        {
            var university = await _context.UniversityRatings
                .Where(r => r.Id == ratingId)
                .FirstOrDefaultAsync();

            return university is not null ? true : false;
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
