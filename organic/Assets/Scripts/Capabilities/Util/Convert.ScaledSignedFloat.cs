namespace Capabilities.Util
{
    public static partial class Convert
    {
        public static float ScaledSignedFloat(ulong input)
        {
            return ((float) input / ULongMidValue) - 1f;
        }
    }
}