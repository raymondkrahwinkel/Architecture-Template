using Application.Common.Interfaces.Messaging;
using Domain;
using Domain.Abstractions;

namespace Application.Common.Services;

public interface ICqrsSender : IScopedService
{
    Task<Result<TResult>?> SendAsync<TResult>(IQuery<TResult> request, CancellationToken cancellationToken = default);
    Task<Result?> SendAsync(ICommand request, CancellationToken cancellationToken = default);
    Task<Result<TResult>?> SendAsync<TResult>(ICommand<TResult> request, CancellationToken cancellationToken = default);
}