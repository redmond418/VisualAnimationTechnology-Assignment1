using UnityEngine;

namespace VAT
{
    public struct ProcessContext
    {
        public int textureWidth;
        public int textureHeight;
        public Color32[] sourcePixels;
        public Color32[] savedUVPixels;
        public Color32[] pixelsCache;
        public int[] deltas;
    }
}
