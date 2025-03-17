namespace Divoom.Tools;

using Divoom.Client;

internal static class ExceptionExtensions
{
    public static void EnsureSuccessStatus(this ServiceResult result)
    {
        if (result.Code != 0)
        {
            throw new DivoomException($"{result.Message} errorCode={result.Code}");
        }
    }

    public static void EnsureSuccessStatus(this DeviceResult result)
    {
        if (result.Code != 0)
        {
            throw new DivoomException($"Execute failed. errorCode={result.Code}");
        }
    }
}
