namespace SealTypographic.Models
{
    public class AccountantSealData
    {
        public string? CaseNo { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string>? SealImagePath { get; set; }
        public List<string>? SignatureImagePath { get; set; }
        public List<string>? DoubleSignatureImagePath { get; set; }
        public List<string>? OldStyleSignatureImagePath { get; set; }
    }
}
