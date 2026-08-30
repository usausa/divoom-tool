namespace Divoom.Tools;

internal sealed class DivoomException : Exception
{
    public DivoomException(string message)
        : base(message)
    {
    }
}
