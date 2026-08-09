namespace Divoom.Client;

public sealed class DivoomClientException : Exception
{
    public DivoomClientException()
    {
    }

    public DivoomClientException(string message)
        : base(message)
    {
    }

    public DivoomClientException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
