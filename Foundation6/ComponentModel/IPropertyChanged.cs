namespace Foundation.ComponentModel;

public interface IPropertyChanged
{
    CollectionActionState ActionState { get; }
    string PropertyName { get; }
}
