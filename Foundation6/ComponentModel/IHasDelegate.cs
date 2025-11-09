namespace Foundation.ComponentModel;

public interface IHasDelegate : IHasDelegate<Delegate>;

public interface IHasDelegate<TDelegate>
	where TDelegate : Delegate
{
    TDelegate Delegate { get; }
}
