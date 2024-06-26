using System.Management;

namespace AutoBenchmarkDownloader.MVVM
{
    internal class SystemHardwareInfo
    {
        public static Dictionary<string, string> GetHardwareInfo(string computerSystemHardwareClass, List<string> infoToGet, string errorInfo)
        {
            var result = new Dictionary<string, string>();

            try
            {
                using (var searcher = new ManagementObjectSearcher($"SELECT * FROM {computerSystemHardwareClass}"))
                {
                    foreach (ManagementObject item in searcher.Get())
                    {
                        foreach (var info in infoToGet)
                        {
                            if (item[info] != null)
                            {
                                result[info] = item[info].ToString();
                            }
                            else
                            {
                                result[info] = $"[{errorInfo} {info} ERROR]";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                foreach (var info in infoToGet)
                {
                    result[info] = $"[{errorInfo} {info} ERROR]";
                }
            }

            return result;
        }
    }
}
