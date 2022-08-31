namespace MarketPlacement.Infrastructure.Repositories;

using Domain.Repositories;
using Domain.Repositories.AuthenticationRepository;

public class EfUnitOfWork : IEfUnitOfWork
{
    private readonly DataBaseContext _ctx;

    public EfUnitOfWork(DataBaseContext ctx, IUserRepository userRepository)
    {
        _ctx = ctx;
        UserRepository = userRepository;
    }

    public IUserRepository UserRepository { get; }

    public async Task SaveAsync() => await _ctx.SaveChangesAsync();
}