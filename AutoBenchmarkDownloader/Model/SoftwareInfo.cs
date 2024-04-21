namespace AutoBenchmarkDownloader.Model
{
    public class SoftwareInfo
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public bool Download { get; set; }
    }
}
