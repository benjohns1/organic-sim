using UnityEngine;

namespace Interactions.Util
{
    public static partial class Convert
    {
        public static Vector3 Vector3(ulong input)
        {
            var x = Signed16BitToScaledFloat(input, 0);
            var y = Signed16BitToScaledFloat(input, 16);
            var z = Signed16BitToScaledFloat(input, 32);
            var m = Unsigned16BitToUInt(input, 48);

            return (new Vector3(x, y, z)) * m;
        }

        public static ulong Vector3(Vector3 output)
        {
            var normalized = output.normalized;

            var x = ScaledFloatToSigned16Bit(normalized.x, 0);
            var y = ScaledFloatToSigned16Bit(normalized.y, 16);
            var z = ScaledFloatToSigned16Bit(normalized.y, 32);
            var m = UIntToUnsigned16Bit((uint) output.magnitude, 48);

            return x | y | z | m;
        }
    }
}