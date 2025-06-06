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
        public Color32[] processedUVPixels;
        public float currentTime;

        public ProcessContext(
            int textureWidth,
            int textureHeight,
            Color32[] sourcePixels,
            Color32[] savedUVPixels,
            Color32[] pixelsCache,
            Color32[] processedUVPixels,
            float currentTime)
        {
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.sourcePixels = sourcePixels;
            this.savedUVPixels = savedUVPixels;
            this.pixelsCache = pixelsCache;
            this.processedUVPixels = processedUVPixels;
            this.currentTime = currentTime;
        }
    }
}
