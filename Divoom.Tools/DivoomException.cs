namespace Divoom.Tools;

#pragma warning disable CA1032
internal sealed class DivoomException : Exception
{
    public DivoomException(string message)
        : base(message)
    {
    }
}
#pragma warning restore CA1032
