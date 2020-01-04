namespace Interactions.Util
{
    public static partial class Convert
    {
        public static float ScaledFloat(ulong input)
        {
            return (float) (long) input / long.MaxValue;
        }
    }
}