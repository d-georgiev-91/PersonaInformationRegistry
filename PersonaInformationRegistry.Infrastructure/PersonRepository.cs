using Microsoft.EntityFrameworkCore;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonaInformationRegistry.Infrastructure;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public PersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        person.MarkAsDeleted();
        await UpdateAsync(person);
    }

    public async Task<IEnumerable<Person>> GetAllAsync(int pageNumber, int pageSize, string? sortOrder = null, string? nameFilter = null)
    {
        var query = _context.People.Include(p => p.Credentials).Where(p => !p.IsDeleted);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(p => p.Name.Contains(nameFilter));
        }

        if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            query = query.OrderByDescending(p => p.Name);
        }
        else
        {
            query = query.OrderBy(p => p.Name);
        }

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People
            .Include(p => p.Credentials)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public Task<int> GetTotalCountAsync(string? nameFilter = null)
    {
        var query = _context.People.Where(p => !p.IsDeleted);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(p => p.Name.Contains(nameFilter));
        }

        return query.CountAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }
}
