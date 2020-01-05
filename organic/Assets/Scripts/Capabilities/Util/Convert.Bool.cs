namespace Capabilities.Util
{
    public static partial class Convert
    {
        public static bool Bool(ulong input)
        {
            return input > ULongMidValue;
        }

        public static ulong Bool(bool output)
        {
            return output ? ulong.MaxValue : ulong.MinValue;
        }
    }
}