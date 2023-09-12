namespace Foundation.DI;

public record Inject<T, TImpl>(T Instance) : IInject<T, TImpl>;
