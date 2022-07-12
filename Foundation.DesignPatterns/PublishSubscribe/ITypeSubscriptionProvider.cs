namespace Foundation.DesignPatterns.PublishSubscribe;

internal interface ITypeSubscriptionProvider : ISubscriptionProvider<Type, Action<object>>
{
}
