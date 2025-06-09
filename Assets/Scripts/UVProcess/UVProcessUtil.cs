using UnityEngine;

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

        public static Vector2 Rotate2D(Vector2 vector, float angle, Vector2 centerPoint = default)
        {
            return (Vector2)(Quaternion.AngleAxis(angle, Vector3.forward) * (vector - centerPoint)) + centerPoint;
        }
    }
}
