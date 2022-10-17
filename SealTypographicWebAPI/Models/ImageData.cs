namespace SealTypographicWebAPI.Models
{
    public class ImageData
    {
        public string? FileName { get; set; }
        public string? FileFullName { get; set; }
        public string ContentType { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
    }
}
