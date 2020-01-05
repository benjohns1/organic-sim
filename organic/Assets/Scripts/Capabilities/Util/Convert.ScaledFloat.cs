using System;

namespace Capabilities.Util
{
    public static partial class Convert
    {
        public static float ScaledFloat(ulong input)
        {
            return (float) input / ulong.MaxValue;
        }
    }
}