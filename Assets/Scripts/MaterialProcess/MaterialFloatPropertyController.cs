using Alchemy.Inspector;
using UnityEngine;

namespace VAT
{
    public class MaterialFloatPropertyController : MonoBehaviour
    {
        [SerializeField] private Renderer m_renderer;
        [SerializeField] private string propertyName;
        [SerializeField, OnValueChanged(nameof(OnValueUpdate))] private float value;

        private Material material;

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueUpdate(value);
            }
        }

        private void Reset()
        {
            m_renderer = GetComponent<Renderer>();
        }

        private void Awake()
        {
            material = m_renderer.material;
            OnValueUpdate(value);
        }

        private void OnValueUpdate(float value)
        {
            if (!Application.isPlaying) return;
            material.SetFloat(propertyName, value);
        }
    }
}
