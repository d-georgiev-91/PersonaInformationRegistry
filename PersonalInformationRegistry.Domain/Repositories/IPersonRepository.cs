using PersonalInformationRegistry.Domain.Entities;

namespace PersonalInformationRegistry.Domain.Repositories;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(int id);

    Task<IEnumerable<Person>> GetAllAsync(int pageNumber, int pageSize, string? sortOrder = null, string? nameFilter = null);

    Task AddAsync(Person person);

    Task UpdateAsync(Person person);

    Task DeleteAsync(Person person);

    Task<int> GetTotalCountAsync(string? nameFilter = null);
}
