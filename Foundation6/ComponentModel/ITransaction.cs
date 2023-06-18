namespace Foundation.ComponentModel;

public interface ITransaction : IDisposable
{
    void Commit();
}