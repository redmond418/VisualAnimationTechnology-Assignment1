namespace VAT
{
    public static class UVProcessUtil
    {
        public static int XYToIndex(int x, int y, int width, int height)
        {
            return ModLoop(x, width) + ModLoop(y, height) * width;
        }

        public static int ModLoop(int a, int b)
        {
            int mod = a % b;
            if (mod < 0) mod += b;
            return mod;
        }
    }
}
