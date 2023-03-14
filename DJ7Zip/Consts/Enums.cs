using System;
using System.Collections.Generic;
using System.Text;

namespace DJ7Zip.Consts
{
    public enum LzmaSpeed : int
    {
        Fastest = 5,
        VeryFast = 8,
        Fast = 16,
        Medium = 32,
        Slow = 64,
        VerySlow = 128,
    }

    public enum DictionarySize : int
    {
        ///<summary>64 KiB</summary>
        VerySmall = 1 << 16,
        ///<summary>1 MiB</summary>
        Small = 1 << 20,
        ///<summary>4 MiB</summary>
        Medium = 1 << 22,
        ///<summary>8 MiB</summary>
        Large = 1 << 23,
        ///<summary>16 MiB</summary>
        Larger = 1 << 24,
        ///<summary>64 MiB</summary>
        VeryLarge = 1 << 26,
    }
}
