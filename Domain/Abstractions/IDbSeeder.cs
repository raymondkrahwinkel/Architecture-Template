namespace Domain.Abstractions;

public interface IDbSeeder : ITransientService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}