namespace Foundation.ComponentModel;

public record struct DictionaryEvent<TKey, TValue>(DictionaryAction Action, KeyValuePair<TKey, TValue>? Element);

