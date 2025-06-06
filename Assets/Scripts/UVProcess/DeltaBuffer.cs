using System;
using UnityEngine;

namespace VAT
{
    public class DeltaBuffer
    {
        private int[] deltas;
        private float[] previousTimeArray;
        private int center;
        private float speed;

        public int[] Deltas => deltas;

        public DeltaBuffer(int count, int center, float speed)
        {
            deltas = new int[count];
            previousTimeArray = new float[count];
            this.center = center;
            this.speed = speed;
        }

        public void ResetTime(float currentTime)
        {
            for (int i = 0; i < previousTimeArray.Length; i++)
            {
                previousTimeArray[i] = currentTime;
            }
        }

        public void Update(float currentTime)
        {
            for (int i = 0; i < previousTimeArray.Length; i++)
            {
                int fromCenter = i - center;
                deltas[i] = (int)((currentTime - previousTimeArray[i]) * fromCenter * speed);
                if (deltas[i] != 0) previousTimeArray[i] = currentTime;
            }
        }
    }

    [Serializable]
    public class DeltaBufferContainer256
    {
        [SerializeField] private int center;
        [SerializeField] private float speed;
        private DeltaBuffer deltaBuffer;

        public DeltaBuffer DeltaBuffer
        {
            get
            {
                if (deltaBuffer == null)
                {
                    deltaBuffer = new DeltaBuffer(256, center, speed);
                }
                return deltaBuffer;
            }
        }
    }
}
