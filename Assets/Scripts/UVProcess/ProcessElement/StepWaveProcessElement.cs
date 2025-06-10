using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class StepWaveProcessElement : IProcessElement
    {
        [SerializeField] private RGBA sourceColor;
        [SerializeField] private Axis waveAxis;
        [SerializeField] private float waveSpeed = 1;
        [SerializeField] private float waveScale = 1;
        [SerializeField] private DeltaBuffersContainer256xn deltaBuffers;

        public void ProcessUV(ProcessContext context)
        {
            for (int i = 0; i < deltaBuffers.DeltaBuffers.Length; i++)
            {
                deltaBuffers.DeltaBuffers[i].Update(Mathf.Sin(context.currentTime * waveSpeed + waveScale * i));
            }
            
            for (int j = 0; j < context.textureHeight; j++)
            {
                for (int i = 0; i < context.textureWidth; i++)
                {
                    int originIndex = UVProcessUtil.XYToIndex(i, j, context.textureWidth, context.textureHeight);
                    Vector2Int indexVector = new(i, j);
                    int waveIndex = indexVector[1 - (int)waveAxis];
                    Vector2Int indexOffset = Vector2Int.zero;
                    indexOffset[(int)waveAxis] = deltaBuffers.DeltaBuffers[waveIndex].Deltas[context.sourcePixels[originIndex][(int)sourceColor]];
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
            int size = waveAxis switch
            {
                Axis.X => 256,
                Axis.Y => 256,
                _ => 256,
            };
            deltaBuffers.Initialize(size);
        }
    }
}
