namespace Application.Common.Interfaces.Messaging;

public interface IQuery<TResult> : IBaseQuery;

public interface IBaseQuery : IRequest;