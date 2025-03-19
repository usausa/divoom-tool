namespace Divoom.Tools;

using Divoom.Client;

internal static class ExceptionExtensions
{
    public static void EnsureSuccessStatus(this ServiceResult result)
    {
        if ((result.ReturnCode != 0) || !String.IsNullOrEmpty(result.ReturnMessage))
        {
            throw new DivoomException($"Execute failed. code=[{result.ReturnCode}], message=[{result.ReturnMessage}]");
        }
    }

    public static void EnsureSuccessStatus(this DeviceResult result)
    {
        if ((result.ErrorCode != 0) || !String.IsNullOrEmpty(result.ErrorMessage))
        {
            throw new DivoomException($"Execute failed. code=[{result.ErrorCode}], message=[{result.ErrorMessage}]");
        }
    }
}
