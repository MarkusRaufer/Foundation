namespace Foundation.Runtime.InteropServices;

using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

public static class FileTime
{
    #region Win32

    [DllImport("kernel32.dll")]
    private static extern void GetSystemTimeAsFileTime(out FILETIME lpSystemTimeAsFileTime);

    #endregion

    public static long GetCurrentUtcTime()
    {
        FILETIME time;
        GetSystemTimeAsFileTime(out time);

        return ((long)time.dwHighDateTime << 32) + (long)(uint)time.dwLowDateTime;
    }
}

