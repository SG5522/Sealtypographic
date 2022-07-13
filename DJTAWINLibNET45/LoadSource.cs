using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTwain;
using NTwain.Data;

namespace DJTAWINLibNET45
{
    public class LoadSource
    {
        public bool GroupDepthEnabled { get; set; }
        public bool GroupDPIEnabled { get; set; }
        public TwainSession NTWAIN { get; set; }
    }
}
