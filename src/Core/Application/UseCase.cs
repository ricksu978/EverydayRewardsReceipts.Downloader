namespace Application
{
    public abstract class UseCase<TRequest, TResponse>
    {
        public abstract Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
    }
}
