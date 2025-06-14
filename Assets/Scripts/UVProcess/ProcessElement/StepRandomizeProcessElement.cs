using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VAT
{
    [Serializable]
    public class StepRandomizeProcessElement : IProcessElement
    {
        [SerializeField] private RGBA sourceColor;

        public void ProcessUV(ProcessContext context)
        {
            for (int j = 0; j < context.textureHeight; j++)
            {
                for (int i = 0; i < context.textureWidth; i++)
                {
                    int originIndex = i + j * context.textureWidth;
                    Color32 uv = context.processedUVPixels[originIndex];
                    if (Random.Range(0, 256) < context.sourcePixels[originIndex][(int)sourceColor])
                    {
                        uv = new((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 0, 255);
                        context.savedUVPixels[originIndex] = uv;
                    }
                    context.pixelsCache[originIndex] = uv;
                }
            }
            context.CacheToProcessedUV();
        }

        public void ResetTime(float currentTime)
        {

        }
    }
}
