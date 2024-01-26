namespace Foundation.ComponentModel;

public record struct CollectionEvent<T>(CollectionAction Action, T? Element);

