using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class StepScrollProcessElement : IProcessElement
    {
        [SerializeField] private RGBA sourceColor;
        [SerializeField] private Axis scrollAxis;
        [SerializeField] private DeltaBufferContainer256 deltaBuffer;

        public void ProcessUV(ProcessContext context)
        {
            deltaBuffer.DeltaBuffer.Update(context.currentTime);
            for (int j = 0; j < context.textureHeight; j++)
            {
                for (int i = 0; i < context.textureWidth; i++)
                {
                    int originIndex = UVProcessUtil.XYToIndex(i, j, context.textureWidth, context.textureHeight);
                    Vector2Int indexOffset = Vector2Int.zero;
                    indexOffset[(int)scrollAxis] = deltaBuffer.DeltaBuffer.Deltas[context.sourcePixels[originIndex][(int)sourceColor]];
                    int uvIndex = UVProcessUtil.XYToIndex(i - indexOffset.x, j - indexOffset.y, context.textureWidth, context.textureHeight);
                    if (context.sourcePixels[uvIndex][(int)sourceColor] != context.sourcePixels[originIndex][(int)sourceColor])
                        uvIndex = UnityEngine.Random.Range(0, context.textureWidth * context.textureHeight);
                    context.pixelsCache[originIndex] = context.processedUVPixels[uvIndex];
                }
            }
            context.CacheToProcessedUV();
            context.SaveUV();
        }

        public void ResetTime(float currentTime)
        {
            deltaBuffer.DeltaBuffer.Update(currentTime);
        }
    }
}
