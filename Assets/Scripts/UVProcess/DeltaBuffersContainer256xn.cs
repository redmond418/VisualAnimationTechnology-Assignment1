using System;
using UnityEngine;

namespace VAT
{
    [Serializable]
    public class DeltaBuffersContainer256xn
    {

        [SerializeField] private int center = 128;
        [SerializeField] private float speed = 1;
        private DeltaBuffer[] deltaBuffers;

        public DeltaBuffer[] DeltaBuffers
        {
            get
            {
                if (deltaBuffers == null)
                {
                    Initialize(256);
                }
                return deltaBuffers;
            }
        }

        public void Initialize(int arraySize)
        {
            deltaBuffers = new DeltaBuffer[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                deltaBuffers[i] = new DeltaBuffer(256, center, speed);
            }
        }
    }
}
