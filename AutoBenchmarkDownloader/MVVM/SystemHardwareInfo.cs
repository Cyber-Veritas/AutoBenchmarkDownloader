using System.Management;

namespace AutoBenchmarkDownloader.MVVM
{
    internal class SystemHardwareInfo
    {
        public static string GetHardwareInfo(string computerSystemHardwareClass, string infoToGet, string errorInfo)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM "+computerSystemHardwareClass+"");
                foreach (ManagementObject item in searcher.Get())
                {
                    return (string)item[infoToGet];
                }
                return "["+errorInfo+" info ERROR]";
            }
            catch (Exception e)
            {
                return "[" + errorInfo + " info ERROR]";
            }
        }
    }
}
