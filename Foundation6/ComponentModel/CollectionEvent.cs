namespace Foundation.ComponentModel;

public record struct CollectionEvent<T>(CollectionChangedState State, T? Element);

