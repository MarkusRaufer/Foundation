namespace Foundation;

public static class Loop
{
    public static void Repeat(int numberOfIterations, Action action)
    {
        for(var i = 0; i< numberOfIterations; i++)
            action();
    }
}
