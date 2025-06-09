using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class UnsaveRotateProcessElement : IProcessElement
    {
        [SerializeField] private RGBA sourceColor;
        [SerializeField] private Vector2 centerPointRatio = new(0.5f, 0.5f);
        [SerializeField] private float angleSpeed;
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
                    float angle = time * angleSpeed * (context.sourcePixels[originIndex][(int)sourceColor] - center);
                    Vector2 centerPoint = centerPointRatio * new Vector2(context.textureWidth, context.textureHeight);
                    Vector2 rotatedUV = UVProcessUtil.Rotate2D(new(i, j), angle, centerPoint);
                    int uvIndex = UVProcessUtil.XYToIndex(Mathf.FloorToInt(rotatedUV.x), Mathf.FloorToInt(rotatedUV.y), context.textureWidth, context.textureHeight);
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
