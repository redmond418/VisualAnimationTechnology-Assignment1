using UnityEngine;

namespace VAT
{
    [CreateAssetMenu(fileName = "Process Elements", menuName = "Process Elements Group")]
    public class ProcessElementsGroup : ScriptableObject
    {
        [SerializeField, SerializeReference] private IProcessElement[] elements;
        public IProcessElement[] Elements => elements;

        public void ProcessAll(ProcessContext context)
        {
            foreach (var element in elements)
            {
                element.ProcessUV(context);
            }
        }

        public void ResetTimeAll(float currentTime)
        {
            foreach (var element in elements)
            {
                element.ResetTime(currentTime);
            }
        }
    }
}
