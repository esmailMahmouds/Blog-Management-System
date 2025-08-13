using BlogApp.Models.DomainClasses;

namespace BlogApp.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        public Task<IEnumerable<Country>> GetCountries();
    }
}
