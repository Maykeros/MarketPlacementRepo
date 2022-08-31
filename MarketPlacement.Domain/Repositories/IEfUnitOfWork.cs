namespace MarketPlacement.Domain.Repositories;

using AuthenticationRepository;

public interface IEfUnitOfWork
{
    IUserRepository UserRepository { get; }

    Task SaveAsync();
}