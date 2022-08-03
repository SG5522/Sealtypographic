namespace SealTypographic.Models
{
    public class Image
    {
        public string? FileName { get; set; }
        public string? FileFullName { get; set; }
        public string ContentType { get; set; } = null!;
        public byte[] Data { get; set; } = null!;        
    }
}
