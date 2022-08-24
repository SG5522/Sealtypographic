using System.Collections.Generic;

namespace DJTWAINLib
{
    public class ScanSourceDataList
    {
        public List<string> LszIdentity = new List<string>();
        public string DefaultScan { get; set; }
        public string ErrorMessage { get; set; }
    }
}
