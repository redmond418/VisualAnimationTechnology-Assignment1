using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class UnsaveTrackProcessElement : IProcessElement
    {
        [SerializeField] private RGBA sourceColorX = RGBA.Red;
        [SerializeField] private RGBA sourceColorY = RGBA.Green;
        [SerializeField] private bool sampleSavedUV = false;

        private float originTime;

        public void ProcessUV(ProcessContext context)
        {
            float time = context.currentTime - originTime;
            for (int j = 0; j < context.textureHeight; j++)
            {
                for (int i = 0; i < context.textureWidth; i++)
                {
                    int originIndex = UVProcessUtil.XYToIndex(i, j, context.textureWidth, context.textureHeight);
                    int mappedX = context.sourcePixels[originIndex][(int)sourceColorX];
                    int mappedY = context.sourcePixels[originIndex][(int)sourceColorY];
                    int uvIndex;
                    Color32[] samplePixels = sampleSavedUV ? context.savedUVPixels : context.processedUVPixels;
                    if (mappedX == 0 && mappedY == 0)
                    {
                        uvIndex = originIndex;
                        samplePixels = context.processedUVPixels;
                    }
                    else
                    {
                        // (0, 0)との区別のため1ずらしていた値を戻す
                        mappedX -= 1;
                        mappedY -= 1;
                        uvIndex = UVProcessUtil.XYToIndex(mappedX, mappedY, context.textureWidth, context.textureHeight);
                    }
                    context.pixelsCache[originIndex] = samplePixels[uvIndex];
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