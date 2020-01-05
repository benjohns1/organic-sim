using System;

namespace Capabilities.Util
{
    public static partial class Convert
    {
        private const ulong MaskUnsigned16Bit = 0b_1111_1111_1111_1111;
        private const ulong MaskSigned16Bit = 0b_0111_1111_1111_1111;
        private const ulong Mask16BitSign = 0b_1000_0000_0000_0000;

        private const uint Max16 = 0b_1111_1111_1111_1111;

        private const ulong ULongMidValue = ulong.MaxValue / 2;

        private static float Signed16BitToScaledFloat(ulong data, int offset)
        {
            var mask = MaskSigned16Bit << offset;
            var signMask = Mask16BitSign << offset;
            var a = (data & mask) >> offset;
            var sign = (data & signMask) == 0 ? 1 : -1;
            return (float) sign * a / Max16;
        }

        private static ulong ScaledFloatToSigned16Bit(float data, int offset)
        {
            var a = (ulong)Math.Abs((int) (data * Max16));
            var val = data > 0 ? a : a | Mask16BitSign;
            return val << offset;
        }

        private static uint Unsigned16BitToUInt(ulong data, int offset)
        {
            var mask = MaskUnsigned16Bit << offset;
            var a = (data & mask) >> offset;
            return (uint) a;
        }

        private static ulong UIntToUnsigned16Bit(uint data, int offset)
        {
            return (data & MaskUnsigned16Bit) << offset;
        }
    }
}