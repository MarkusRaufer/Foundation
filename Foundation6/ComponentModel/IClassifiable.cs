namespace Foundation.ComponentModel;

/// <summary>
/// This can be used to classify an object like entity, value object.
/// </summary>
/// <typeparam name="TStereoType"></typeparam>
public interface IClassifiable<TStereoType>
{
    TStereoType StereoType { get; }
}
