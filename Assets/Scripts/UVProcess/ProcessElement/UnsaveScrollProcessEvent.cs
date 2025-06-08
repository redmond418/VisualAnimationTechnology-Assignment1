using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class UnsaveScrollProcessEvent : IProcessElement
    {
        [SerializeField] private RGBA sourceColor;
        [SerializeField] private Vector2 velocity;
        [SerializeField] private int center = 128;

        private float originTime;

        public void ProcessUV(ProcessContext context)
        {
            float time = context.currentTime - originTime;
            for (int j = 0; j < context.textureHeight; j++)
            {
                for (int i = 0; i < context.textureWidth; i++)
                {
                    int originIndex = UVProcessUtil.XYToIndex(i, j, context.textureWidth, context.textureHeight);
                    Vector2 indexOffset = time * velocity * (context.sourcePixels[originIndex][(int)sourceColor] - center);
                    int uvIndex = UVProcessUtil.XYToIndex(i - (int)indexOffset.x, j - (int)indexOffset.y, context.textureWidth, context.textureHeight);
                    context.pixelsCache[originIndex] = context.processedUVPixels[uvIndex];
                }
            }
            context.CacheToProcessedUV();
        }

        public void ResetTime(float currentTime)
        {
            originTime = currentTime;
        }
    }
}
